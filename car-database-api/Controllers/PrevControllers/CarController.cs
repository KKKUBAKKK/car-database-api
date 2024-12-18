// using car_database_api.Auth;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using car_database_api.Models;
// using car_database_api.Data;
// using Microsoft.AspNetCore.Authorization;
//
// namespace car_database_api.Controllers
// {
//     [ApiController]
//     [Route("api/customer/[controller]")]
//     [Authorize(Roles = Roles.Customer)]
//     public class CarController : ControllerBase
//     {
//         private readonly CarRentalDbContext _context;
//         // private readonly IMapper _mapper;
//
//         public CarController(CarRentalDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: api/Car
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Car>>> GetCars()
//         {
//             return await _context.Cars.ToListAsync();
//         }
//
//         // GET: api/Car/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Car>> GetCar(int id)
//         {
//             var car = await _context.Cars.FindAsync(id);
//
//             if (car == null)
//             {
//                 return NotFound();
//             }
//
//             return car;
//         }
//
//         // PUT: api/Car/5
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutCar(int id, Car car)
//         {
//             if (id != car.CarID)
//             {
//                 return BadRequest();
//             }
//
//             _context.Entry(car).State = EntityState.Modified;
//
//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!CarExists(id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }
//
//             return NoContent();
//         }
//
//         // POST: api/Car
//         [HttpPost]
//         public async Task<ActionResult<Car>> PostCar(Car car)
//         {
//             _context.Cars.Add(car);
//             await _context.SaveChangesAsync();
//
//             return CreatedAtAction(nameof(GetCar), new { id = car.CarID }, car);
//         }
//
//         // DELETE: api/Car/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteCar(int id)
//         {
//             var car = await _context.Cars.FindAsync(id);
//             if (car == null)
//             {
//                 return NotFound();
//             }
//
//             _context.Cars.Remove(car);
//             await _context.SaveChangesAsync();
//
//             return NoContent();
//         }
//
//         private bool CarExists(int id)
//         {
//             return _context.Cars.Any(e => e.CarID == id);
//         }
//     }
// }