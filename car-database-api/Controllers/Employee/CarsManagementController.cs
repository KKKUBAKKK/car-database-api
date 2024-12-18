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
[Route("api/employee/cars")]
[Authorize(Roles = Roles.Employee)]
public class CarsManagementController : ControllerBase
{
    private readonly CarRentalDbContext _context;
    private readonly IMapper _mapper;

    public CarsManagementController(CarRentalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetAllCars()
    {
        var cars = await _context.Cars
            // .Include(c => c.Rentals)
            .ToListAsync();
        
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }

    [HttpPost]
    public async Task<ActionResult<CarDto>> AddCar([FromBody] CarCreateDto car)
    {
        var newCar = _mapper.Map<Car>(car);
        _context.Cars.Add(newCar);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetCar), new { newCar.id }, 
            _mapper.Map<CarDto>(newCar));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CarDto>> UpdateCar(int id, [FromBody] CarUpdateDto car)
    {
        var existingCar = await _context.Cars.FindAsync(id);
        if (existingCar == null)
        {
            return NotFound();
        }

        _mapper.Map(car, existingCar);
        await _context.SaveChangesAsync();
        
        return Ok(_mapper.Map<CarDto>(existingCar));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCar(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        if (car.isAvailable == false)
        {
            return BadRequest("Car is currently rented out");
        }
        
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarDto>> GetCar(int id)
    {
        var car = await _context.Cars
            // .Include(c => c.Rentals)
            .FirstOrDefaultAsync(c => c.id == id);

        if (car == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CarDto>(car));
    }
}
