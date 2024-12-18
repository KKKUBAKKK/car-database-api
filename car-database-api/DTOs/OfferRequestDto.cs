namespace car_database_api.DTOs;

public class OfferRequestDto
{
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public DateOnly birthday { get; set; }
    public DateOnly driverLicenseReceiveDate { get; set; }
    public string RentalName { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
}