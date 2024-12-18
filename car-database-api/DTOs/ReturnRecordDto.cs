namespace car_database_api.DTOs;

public class ReturnRecordDto
{
    // public int Id { get; set; }
    public int RentalId { get; set; }
    public string Condition { get; set; }
    // public string PhotoUrl { get; set; }
    public string EmployeeNotes { get; set; }
    public DateTime ReturnDate { get; set; }
}