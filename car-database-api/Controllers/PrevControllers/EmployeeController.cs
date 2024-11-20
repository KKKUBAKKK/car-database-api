// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using car_database_api.Models;
// using car_database_api.Data;
//
// namespace car_database_api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class EmployeeController : ControllerBase
//     {
//         private readonly CarRentalDbContext _context;
//
//         public EmployeeController(CarRentalDbContext context)
//         {
//             _context = context;
//         }
//
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
//         {
//             return await _context.Employees.ToListAsync();
//         }
//
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Employee>> GetEmployee(int id)
//         {
//             var employee = await _context.Employees.FindAsync(id);
//
//             if (employee == null)
//             {
//                 return NotFound();
//             }
//
//             return employee;
//         }
//
//         [HttpPost]
//         public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
//         {
//             _context.Employees.Add(employee);
//             await _context.SaveChangesAsync();
//
//             return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
//         }
//
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutEmployee(int id, Employee employee)
//         {
//             if (id != employee.EmployeeID)
//             {
//                 return BadRequest();
//             }
//
//             _context.Entry(employee).State = EntityState.Modified;
//
//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!EmployeeExists(id))
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
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteEmployee(int id)
//         {
//             var employee = await _context.Employees.FindAsync(id);
//             if (employee == null)
//             {
//                 return NotFound();
//             }
//
//             _context.Employees.Remove(employee);
//             await _context.SaveChangesAsync();
//
//             return NoContent();
//         }
//
//         private bool EmployeeExists(int id)
//         {
//             return _context.Employees.Any(e => e.EmployeeID == id);
//         }
//     }
// }