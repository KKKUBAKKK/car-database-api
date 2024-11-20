using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using car_database_api.Helpers;

namespace car_database_api.Models;

public class User
{
    [Key]
    public int id { get; set; }                                       // Unique identifier for each customer
    
    [Required]
    public int externalId { get; set; }                                    // For OAuth/OpenID
    
    public string rentalName { get; set; } = Constants.RentalName;                                 // Rental's name
    
    [Required, StringLength(50)]
    public string firstName { get; set; }                                     // Customer's first name

    [Required, StringLength(50)]
    public string lastName { get; set; }                                      // Customer's last name

    [Required]
    public DateOnly birthday { get; set; }                                 // Customer's date of birth (for age calculation)
    
    [Required]
    public DateOnly driverLicenseReceiveDate { get; set; }                 // Date when the driver's license was received

    // [Required, StringLength(50)]
    // public string DriverLicenseNumber { get; set; }                           // Unique driver's license number
    
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();    // Navigation properties
}