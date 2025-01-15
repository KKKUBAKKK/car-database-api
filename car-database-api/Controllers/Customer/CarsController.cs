using AutoMapper;
using car_database_api.Auth;
using car_database_api.Data;
using car_database_api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_database_api.Controllers.Customer;

[ApiController]
[Route("api/customer/cars")]
[Authorize(Roles = Roles.Customer)]
public class CarsController(CarRentalDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
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
        if (car == null)
        {
            return NotFound();
        }
        return mapper.Map<CarDto>(car);
    }
}
