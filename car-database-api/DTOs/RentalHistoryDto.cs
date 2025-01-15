using car_database_api.Models;

namespace car_database_api.DTOs;

public class RentalHistoryDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Condition { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; }
    public string RenterName { get; set; }
}
