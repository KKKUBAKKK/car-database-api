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
    
    // Get x cars with id > lastId
    [HttpGet("next/{lastId:int}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetNextCars(int lastId)
    {
        var cars = await _context.Cars
            .Where(c => c.id > lastId)
            .OrderBy(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id < firstId
    [HttpGet("previous/{firstId:int}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetPrevCars(int firstId)
    {
        var cars = await _context.Cars
            .Where(c => c.id < firstId)
            .OrderByDescending(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get total number of cars
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCarsCount()
    {
        var count = await _context.Cars.CountAsync();
        return Ok(count);
    }
    
    // Get x cars with id > lastId and filtered by category
    [HttpGet("next/{lastId:int}/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetNextCars(int lastId, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.id > lastId && c.type == type)
            .OrderBy(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id < firstId and filtered by category
    [HttpGet("previous/{firstId:int}/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetPrevCars(int firstId, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.id < firstId && c.type == type)
            .OrderByDescending(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id > lastId and filtered by category and availability
    [HttpGet("next/available/{lastId:int}/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetNextAvailableCars(int lastId, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.id > lastId && c.type == type && c.isAvailable)
            .OrderBy(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id < firstId and filtered by category and availability
    [HttpGet("previous/available/{firstId:int}/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetPrevAvailableCars(int firstId, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.id < firstId && c.type == type && c.isAvailable)
            .OrderByDescending(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id > lastId and filtered by category and availability
    [HttpGet("next/unavailable/{lastId:int}/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetNextUnavailableCars(int lastId, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.id > lastId && c.type == type && !c.isAvailable)
            .OrderBy(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id < firstId and filtered by category and availability
    [HttpGet("previous/unavailable/{firstId:int}/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetPrevUnavailableCars(int firstId, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.id < firstId && c.type == type && !c.isAvailable)
            .OrderByDescending(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id > lastId and filtered by availability
    [HttpGet("next/available/{lastId:int}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetNextAvailableCars(int lastId)
    {
        var cars = await _context.Cars
            .Where(c => c.id > lastId && c.isAvailable)
            .OrderBy(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id < firstId and filtered by availability
    [HttpGet("previous/available/{firstId:int}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetPrevAvailableCars(int firstId)
    {
        var cars = await _context.Cars
            .Where(c => c.id < firstId && c.isAvailable)
            .OrderByDescending(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id > lastId and filtered by availability
    [HttpGet("next/unavailable/{lastId:int}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetNextUnavailableCars(int lastId)
    {
        var cars = await _context.Cars
            .Where(c => c.id > lastId && !c.isAvailable)
            .OrderBy(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with id < firstId and filtered by availability
    [HttpGet("previous/unavailable/{firstId:int}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetPrevUnavailableCars(int firstId)
    {
        var cars = await _context.Cars
            .Where(c => c.id < firstId && !c.isAvailable)
            .OrderByDescending(c => c.id)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get count of cars filtered by category
    [HttpGet("count/{type}")]
    public async Task<ActionResult<int>> GetCarsCount(CarType type)
    {
        var count = await _context.Cars
            .Where(c => c.type == type)
            .CountAsync();
            
        return Ok(count);
    }
    
    // Get count of cars filtered by category and availability
    [HttpGet("count/{type}/{availability}")]
    public async Task<ActionResult<int>> GetCarsCount(CarType type, bool availability)
    {
        var count = await _context.Cars
            .Where(c => c.type == type && c.isAvailable == availability)
            .CountAsync();
            
        return Ok(count);
    }
    
    // Get count of cars filtered by availability
    [HttpGet("count/availability/{availability}")]
    public async Task<ActionResult<int>> GetCarsCount(bool availability)
    {
        var count = await _context.Cars
            .Where(c => c.isAvailable == availability)
            .CountAsync();
            
        return Ok(count);
    }
    
    // Get x cars with seearch term in name
    [HttpGet("search/{term}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsBySearchTerm(string term)
    {
        var cars = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term))
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with seearch term in name and filtered by category
    [HttpGet("search/{term}/type/{type}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsBySearchTerm(string term, CarType type)
    {
        var cars = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term) && c.type == type)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with seearch term in name and filtered by availability
    [HttpGet("search/{term}/availability/{availability}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsBySearchTerm(string term, bool availability)
    {
        var cars = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term) && c.isAvailable == availability)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get x cars with seearch term in name and filtered by category and availability
    [HttpGet("search/{term}/{type}/{availability}")]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsBySearchTerm(string term, CarType type, bool availability)
    {
        var cars = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term) && c.type == type && c.isAvailable == availability)
            .Take(10)
            .ToListAsync();
            
        return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
    }
    
    // Get count of cars with seearch term in name
    [HttpGet("count/search/{term}")]
    public async Task<ActionResult<int>> GetCarsCountBySearchTerm(string term)
    {
        var count = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term))
            .CountAsync();
            
        return Ok(count);
    }
    
    // Get count of cars with seearch term in name and filtered by category
    [HttpGet("count/search/{term}/type/{type}")]
    public async Task<ActionResult<int>> GetCarsCountBySearchTerm(string term, CarType type)
    {
        var count = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term) && c.type == type)
            .CountAsync();
            
        return Ok(count);
    }
    
    // Get count of cars with seearch term in name and filtered by availability
    [HttpGet("count/search/{term}/availability/{availability}")]
    public async Task<ActionResult<int>> GetCarsCountBySearchTerm(string term, bool availability)
    {
        var count = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term) && c.isAvailable == availability)
            .CountAsync();
            
        return Ok(count);
    }
    
    // Get count of cars with seearch term in name and filtered by category and availability
    [HttpGet("count/search/{term}/{type}/{availability}")]
    public async Task<ActionResult<int>> GetCarsCountBySearchTerm(string term, CarType type, bool availability)
    {
        var count = await _context.Cars
            .Where(c => c.producer.Contains(term) || c.model.Contains(term) && c.type == type && c.isAvailable == availability)
            .CountAsync();
            
        return Ok(count);
    }
}
