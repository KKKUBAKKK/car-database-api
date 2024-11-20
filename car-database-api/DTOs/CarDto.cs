using car_database_api.Models;
using Constants = car_database_api.Helpers.Constants;

namespace car_database_api.DTOs;

public class CarDto
{
    public int Id { get; set; }
    public string RentalService = Constants.RentalName;
    public string Producer { get; set; }
    public string Model { get; set; }
    public int YearOfProduction { get; set; }
    public int NumberOfSeats { get; set; }
    public CarType Type { get; set; }
    public bool IsAvailable { get; set; }
    // public GearboxType Gearbox { get; set; }
    // public FuelType Fuel { get; set; }
    public string Location { get; set; }
}

// public class CarDetailsDto : CarDto
// {
//     public bool IsAvailable { get; set; }
//     public ICollection<RentalHistoryDto> RentalHistory { get; set; }
// }