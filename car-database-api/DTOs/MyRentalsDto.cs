using car_database_api.Helpers;

namespace car_database_api.DTOs;

public class MyRentalsDto
{
    public int CustomerId { get; set; }
    public string RentalName { get; set; } = Constants.RentalName;
}