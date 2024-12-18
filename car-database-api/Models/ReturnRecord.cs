using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_database_api.Models;

public class ReturnRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int RentalId { get; set; }
    public int EmployeeID { get; set; }
    public string Condition { get; set; }
    // public string PhotoUrl { get; set; }
    public string EmployeeNotes { get; set; }
    public DateTime ReturnDate { get; set; }
    
    // Navigation property
    public Rental Rental { get; set; }
    public Employee Employee { get; set; }
}