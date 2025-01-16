using car_database_api.Models;

namespace car_database_api.DTOs;

public class CarUpdateDto
{
    public string Location { get; set; } = "Plac Politechniki, Warszawa";
    public bool IsAvailable { get; set; } = true;
    public CarType Type { get; set; } = CarType.economy;
}