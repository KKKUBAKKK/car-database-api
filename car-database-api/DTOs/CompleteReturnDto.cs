namespace car_database_api.DTOs;

public class CompleteReturnDto
{
    public int UserId { get; set; }
    public string EmployeeNotes { get; set; }
    public DateTime ReturnDate { get; set; }
}