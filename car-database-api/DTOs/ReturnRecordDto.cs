using Microsoft.AspNetCore.Mvc;

namespace car_database_api.DTOs;

public class ReturnRecordDto
{
    // public int Id { get; set; }
    public int EmployeeID { get; set; }
    public int RentalId { get; set; }
    public string Condition { get; set; } = "Good";
    public string FrontPhotoUrl { get; set; } = String.Empty;
    public string BackPhotoUrl { get; set; } = String.Empty;

    public string RightPhotoUrl { get; set; } = String.Empty;

    public string LeftPhotoUrl { get; set; } = String.Empty;
    public string EmployeeNotes { get; set; } = String.Empty;
    public DateTime ReturnDate { get; set; } = DateTime.Now;
}