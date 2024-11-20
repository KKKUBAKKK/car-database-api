using AutoMapper;
using car_database_api.Auth;
using car_database_api.Data;
using car_database_api.DTOs;
using car_database_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_database_api.Controllers.User;

[ApiController]
[Route("api/user/cars")]
// [Authorize(Roles = Roles.Customer)]
public class CarsController(CarRentalDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous] // Allow unregistered users to browse cars
    public async Task<ActionResult<IEnumerable<CarDto>>> GetAvailableCars()
    {
        var cars = await context.Cars
            .Where(c => c.isAvailable)
            .ToListAsync();
            
        return Ok(mapper.Map<IEnumerable<CarDto>>(cars));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<CarDto>> GetCar(int id)
    {
        var car = await context.Cars.FindAsync(id);
        if (car == null || !car.isAvailable)
        {
            return NotFound();
        }
        return mapper.Map<CarDto>(car);
    }
}
