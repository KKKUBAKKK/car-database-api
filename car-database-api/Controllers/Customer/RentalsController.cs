using AutoMapper;
using car_database_api.Auth;
using car_database_api.Data;
using car_database_api.DTOs;
using car_database_api.Helpers;
using car_database_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_database_api.Controllers.Customer;

[ApiController]
[Route("api/customer/rentals")]
[Authorize(Roles = Roles.Customer)]
public class RentalsController(CarRentalDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetMyRentals(MyRentalsDto myRentalsDto)
    {
        // var userId = Customer.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var externalUserId = myRentalsDto.CustomerId;
        var rentalName = myRentalsDto.RentalName;
        var rentals = await context.Rentals
            .Include(r => r.Car)
            .Where(r => r.Customer.externalId == externalUserId && r.Customer.rentalName == rentalName)
            .ToListAsync();
            
        return Ok(mapper.Map<IEnumerable<RentalDto>>(rentals));
    }

    [HttpPost("offers")]
    public async Task<ActionResult<RentalOfferDto>> RequestOffer([FromBody] OfferRequestDto request)
    {
        var customer = await context.Users
            .FirstOrDefaultAsync(u => u.externalId == request.CustomerId && u.rentalName == request.RentalName);

        if (customer == null)
        {
            // Add new customer to database
            customer = new Models.Customer
            {
                externalId = request.CustomerId,
                firstName = request.firstName,
                lastName = request.lastName,
                birthday = request.birthday,
                driverLicenseReceiveDate = request.driverLicenseReceiveDate,
                rentalName = request.RentalName
            };
            context.Users.Add(customer);
            await context.SaveChangesAsync();
        }
        
        var car = await context.Cars.FindAsync(request.CarId);
        if (car == null || !car.isAvailable)
        {
            return NotFound("Car not available");
        }

        
        customer = await context.Users
            .FirstOrDefaultAsync(u => u.externalId == request.CustomerId && u.rentalName == request.RentalName);
        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        var offer = new RentalOffer
        {
            userId = customer.id,
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
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.externalId == request.CustomerId && u.rentalName == request.RentalName);
        if (user == null)
        {
            return NotFound("Customer not found");
        }
        
        var offer = await context.RentalOffers
            .FirstOrDefaultAsync(o => o.id == request.OfferId && o.isActive && o.validUntil > DateTime.UtcNow);

        if (offer == null)
        {
            return NotFound("Offer not found or expired");
        }

        var rental = new Rental
        {
            carId = offer.carId,
            userId = request.CustomerId,
            rentalName = user.rentalName,
            startDate = request.PlannedStartDate,
            endDate = request.PlannedEndDate,
            totalPrice = (offer.dailyRate + offer.insuranceRate) * 
                         (request.PlannedEndDate - request.PlannedStartDate).Days,
            status = request.PlannedStartDate > DateTime.UtcNow ? RentalStatus.planned : RentalStatus.inProgress
        };
        
        var car = await context.Cars.FindAsync(rental.carId);
        if (car == null)
        {
            return NotFound("Car not found");
        }
        
        car.isAvailable = false;
        context.Rentals.Add(rental);
        offer.isActive = false;
        await context.SaveChangesAsync();

        return Ok(rental);
    }
    
    [HttpPost("return")]
    public async Task<ActionResult<ReturnRecordDto>> ReturnCar([FromBody] ReturnRequestDto request)
    {
        var rental = await context.Rentals
            // .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.id == request.RentalId && r.status == RentalStatus.inProgress);
    
        if (rental == null)
        {
            rental = await context.Rentals
                // .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.id == request.RentalId && r.status == RentalStatus.pendingReturn);
            if (rental != null)
            {
                return Ok("Rental already pending return");
            }
            
            return NotFound("Rental not found or not in progress");
        }
    
        rental.status = RentalStatus.pendingReturn;
    
        await context.SaveChangesAsync();
    
        return Ok(rental);
    }
}