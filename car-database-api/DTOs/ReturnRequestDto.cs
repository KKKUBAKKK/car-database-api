namespace car_database_api.DTOs;

public class ReturnRequestDto
{
    public int RentalId { get; set; }
    public string Condition { get; set; }
    public IFormFile Photo { get; set; }
    public string EmployeeNotes { get; set; }
}