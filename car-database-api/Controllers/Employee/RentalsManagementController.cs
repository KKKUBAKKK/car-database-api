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
    private readonly HttpClient _httpClient;

    public RentalsManagementController(CarRentalDbContext context, IMapper mapper, HttpClient httpClient)
    {
        _context = context;
        _mapper = mapper;
        _httpClient = httpClient;
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
    
        var returnRecord = new ReturnRecord
        {
            RentalId = rental.id,
            EmployeeID = request.EmployeeID,
            Condition = request.Condition,
            FrontPhotoUrl = request.FrontPhotoUrl,
            BackPhotoUrl = request.BackPhotoUrl,
            RightPhotoUrl = request.RightPhotoUrl,
            LeftPhotoUrl = request.LeftPhotoUrl,
            EmployeeNotes = request.EmployeeNotes,
            ReturnDate = request.ReturnDate
        };
    
        rental.status = RentalStatus.ended;
        rental.endDate = request.ReturnDate;
        rental.Car.isAvailable = true;
    
        _context.ReturnRecords.Add(returnRecord);
        
        var car = await _context.Cars.FindAsync(rental.carId);
        if (car == null)
        {
            return NotFound("Car not found");
        }
        car.isAvailable = true;
        
        await _context.SaveChangesAsync();
        
        // wyslij wiadomosc do backendu wyszukiwarki z potwierdzeniem zwrotu za pomoca baseUrl w CustomerApi
        var customerApi = await _context.CustomerApis.FirstOrDefaultAsync(ca => ca.username == rental.rentalName);
        if (customerApi == null)
        {
            return NotFound("Customer API not found");
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.id == rental.userId);

        var endpoint = customerApi.baseUrl + "api/cars/return/confirmation";
        var completeReturnDto = new CompleteReturnDto
        {
            UserId = user.externalId,
            EmployeeNotes = returnRecord.EmployeeNotes,
            ReturnDate = returnRecord.ReturnDate
        };
        
        var response = await _httpClient.PostAsJsonAsync(endpoint, completeReturnDto);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
    
        return Ok(_mapper.Map<ReturnRecordDto>(returnRecord));
    }
    
    // Get rentals for a specific vehicle
    [HttpGet("{vehicleId}")]
    public async Task<ActionResult<IEnumerable<RentalDto>>> GetVehicleRentals(int vehicleId)
    {
        var rentals = await _context.Rentals
            .Include(r => r.Car)
            .Include(r => r.Customer)
            .Where(r => r.carId == vehicleId)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<RentalDto>>(rentals));
    }
    
    // Get rental records for a specific vehicle
    [HttpGet("records/{vehicleId}")]
    public async Task<ActionResult<IEnumerable<ReturnRecordDto>>> GetVehicleReturnRecords(int vehicleId)
    {
        var returnRecords = await _context.ReturnRecords
            .Include(rr => rr.Rental)
            .Include(rr => rr.Rental.Car)
            .Include(rr => rr.Rental.Customer)
            .Where(rr => rr.Rental.carId == vehicleId)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ReturnRecordDto>>(returnRecords));
    }
    
    // Get rental history dto for a specific vehicle
    // TODO: fix this, right now it returns the same history for all vehicles
    [HttpGet("history/{vehicleId}")]
    public async Task<ActionResult<RentalHistoryDto>> GetVehicleRentalHistory(int vehicleId)
    {
        var rentalHistories = await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.ReturnRecord)
            .Select(r => new RentalHistoryDto
            {
                StartDate = r.startDate,
                EndDate = r.endDate,
                Condition = (r.ReturnRecord != null) ? r.ReturnRecord.Condition:"Not completed",
                TotalPrice = r.totalPrice,
                Status = r.status,
                RenterName = r.Customer.firstName + " " + r.Customer.lastName
            })
            .ToListAsync();
        return Ok(rentalHistories);
    }
}