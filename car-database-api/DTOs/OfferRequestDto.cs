namespace car_database_api.DTOs;

public class OfferRequestDto
{
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
}