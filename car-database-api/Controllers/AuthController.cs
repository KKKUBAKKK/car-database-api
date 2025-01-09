using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using car_database_api.Auth;
using car_database_api.Data;
using car_database_api.DTOs;
using car_database_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace car_database_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CarRentalDbContext _context;
    
    public AuthController(IConfiguration configuration, CarRentalDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var customerApi = await _context.CustomerApis
            .FirstOrDefaultAsync(c => c.username == login.Username && c.password == login.Password);
        if (customerApi != null)
        {
            var token = GenerateJwtToken(customerApi.username, Roles.Customer);
            return Ok(new { token });
        }
        
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.username == login.Username && e.password == login.Password);
        if (employee != null)
        {
            var token = GenerateJwtToken(employee.username, Roles.Employee);
            var res = new AcceptLoginDto();
            res.token = token;
            res.employeeId = employee.id;
            return Ok(res);
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(string username, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"] ?? string.Empty);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}