namespace car_database_api.DTOs;

public class RentalDto
{
    public int Id { get; set; } // Zwracaj moje id
    public int CarId { get; set; } // Zwracaj moje id
    public int UserId { get; set; } // Zwracaj externalId
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public String StartLocation { get; set; }
    public String EndLocation { get; set; }
}