using AutoMapper;
using car_database_api.Auth;
using car_database_api.Data;
using car_database_api.DTOs;
using car_database_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_database_api.Controllers.Employee;

[ApiController]
[Route("api/employee/rentals")]
[Authorize(Roles = Roles.Employee)]
public class RentalsManagementController : ControllerBase
{
    private readonly CarRentalDbContext _context;
    private readonly IMapper _mapper;

    public RentalsManagementController(CarRentalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetAllRentals([FromQuery] RentalFilterDto filter)
    {
        var query = _context.Rentals
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .AsQueryable();

        if (filter.Status.HasValue)
        {
            query = query.Where(r => r.status == filter.Status);
        }

        var rentals = await query.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<RentalDto>>(rentals));
    }

    [HttpGet("pending-returns")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetPendingReturns()
    {
        var rentals = await _context.Rentals
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.status == RentalStatus.pendingReturn)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<RentalDto>>(rentals));
    }

    [HttpPost("complete-return")]
    public async Task<ActionResult<ReturnRecordDto>> CompleteReturn([FromForm] ReturnRecordDto request)
    {
        var rental = await _context.Rentals
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.id == request.RentalId && r.status == RentalStatus.pendingReturn);
    
        if (rental == null)
        {
            return NotFound("Rental not found or not in pending return status");
        }
    
        // Handle photo upload
        // string photoUrl = await UploadPhotoToBlob(request.Photo);
    
        var returnRecord = new ReturnRecord
        {
            RentalId = rental.id,
            EmployeeID = 1,
            Condition = request.Condition,
            // PhotoUrl = photoUrl,
            EmployeeNotes = request.EmployeeNotes,
            ReturnDate = DateTime.UtcNow
        };
    
        rental.status = RentalStatus.ended;
        rental.endDate = DateTime.UtcNow;
        rental.Car.isAvailable = true;
    
        _context.ReturnRecords.Add(returnRecord);
        
        var car = await _context.Cars.FindAsync(rental.carId);
        if (car == null)
        {
            return NotFound("Car not found");
        }
        car.isAvailable = true;
        
        await _context.SaveChangesAsync();
    
        return Ok(_mapper.Map<ReturnRecordDto>(returnRecord));
    }
    
    // private async Task<string> UploadPhotoToBlob(IFormFile photo)
    // {
    //     // Implement photo upload to Azure Blob Storage
    //     throw new NotImplementedException();
    // }
}