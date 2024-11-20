using System.Security.Claims;
using AutoMapper;
using car_database_api.Auth;
using car_database_api.Data;
using car_database_api.DTOs;
using car_database_api.Helpers;
using car_database_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_database_api.Controllers.User;

[ApiController]
[Route("api/user/rentals")]
// [Authorize(Roles = Roles.Customer)]
public class RentalsController(CarRentalDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetMyRentals(MyRentalsDto myRentalsDto)
    {
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var externalUserId = myRentalsDto.UserId;
        var rentalName = myRentalsDto.RentalName;
        var rentals = await context.Rentals
            .Include(r => r.Car)
            .Where(r => r.User.externalId == externalUserId && r.User.rentalName == rentalName)
            .ToListAsync();
            
        return Ok(mapper.Map<IEnumerable<RentalDto>>(rentals));
    }

    [HttpPost("offers")]
    public async Task<ActionResult<RentalOfferDto>> RequestOffer([FromBody] OfferRequestDto request)
    {
        // Implementation remains similar, but add user verification
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (request.CustomerId.ToString() != userId)
        {
            return Forbid();
        }
        
        var car = await context.Cars.FindAsync(request.CarId);
        if (car == null || car.isAvailable)
        {
            return NotFound("Car not available");
        }
        
        var customer = await context.Users.FindAsync(request.CustomerId);
        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        var offer = new RentalOffer
        {
            carId = request.CarId,
            dailyRate = CarRentalCalculator.CalculateDailyCarRate(car, customer),
            insuranceRate = CarRentalCalculator.CalculateDailyInsuranceRate(car, customer),
            validUntil = DateTime.UtcNow.AddMinutes(10),
            isActive = true
        };

        context.RentalOffers.Add(offer);
        await context.SaveChangesAsync();

        return Ok(offer);
    }

    [HttpPost]
    public async Task<ActionResult<RentalDto>> CreateRental([FromBody] RentalRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (request.CustomerId.ToString() != userId)
        {
            return Forbid();
        }
        
        var offer = await context.RentalOffers
            .FirstOrDefaultAsync(o => o.id == request.OfferId && o.isActive && o.validUntil > DateTime.UtcNow);

        if (offer == null)
        {
            return BadRequest("Invalid or expired offer");
        }

        var rental = new Rental
        {
            carId = offer.carId,
            userId = request.CustomerId,
            startDate = request.PlannedStartDate,
            totalPrice = (offer.dailyRate + offer.insuranceRate) * 
                         (request.PlannedEndDate - request.PlannedStartDate).Days,
            status = request.PlannedStartDate > DateTime.UtcNow ? RentalStatus.planned : RentalStatus.inProgress
        };

        context.Rentals.Add(rental);
        offer.isActive = false;
        await context.SaveChangesAsync();

        return Ok(rental);
    }
}