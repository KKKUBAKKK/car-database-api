using car_database_api.Models;

namespace car_database_api.DTOs;

public class RentalDetailsDto
{
    public int Id { get; set; }
    public CarDto Car { get; set; }
    public UserDto Customer { get; set; }
    public ReturnRecordDto ReturnRecord { get; set; }
    public RentalStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalPrice { get; set; }
}