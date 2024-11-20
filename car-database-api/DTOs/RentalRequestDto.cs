namespace car_database_api.DTOs;

public class RentalRequestDto
{
    public int OfferId { get; set; } // Moje id
    public int CustomerId { get; set; } // External id
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
}