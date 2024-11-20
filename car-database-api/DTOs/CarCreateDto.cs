using System.ComponentModel.DataAnnotations;
using car_database_api.Models;

namespace car_database_api.DTOs;

public class CarCreateDto
{
    [Required]
    public string Producer { get; set; }
    
    [Required]
    public string Model { get; set; }
    
    [Required]
    [Range(1900, 2100)]
    public int YearOfProduction { get; set; }
    
    [Required]
    public int NumberOfSeats { get; set; }
    
    [Required]
    public CarType Type { get; set; }
    
    public string Location { get; set; } = "Plac Politechniki, Warszawa";
}