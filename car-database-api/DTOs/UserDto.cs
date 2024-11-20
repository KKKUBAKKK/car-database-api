using car_database_api.Helpers;

namespace car_database_api.DTOs;

public class UserDto
{
    public int Id { get; set; } // Zwracaj external id
    public string RentalName { get; set; } = Constants.RentalName;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly Birthday { get; set; }
    public DateOnly DriverLicenseReceiveDate { get; set; }
    // public string DriverLicenseNumber { get; set; }
}