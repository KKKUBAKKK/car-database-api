using car_database_api.Models;

namespace car_database_api.DTOs;

public class RentalFilterDto
{
    public RentalStatus? Status { get; set; }
    // public DateTime? StartDate { get; set; }
    // public DateTime? EndDate { get; set; }
}