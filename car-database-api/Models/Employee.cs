using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_database_api.Models;

public class Employee
{
    [Key]
    public int id { get; set; }                    // Unique identifier for each employee

    [Required, StringLength(50)]
    public string firstName { get; set; }                   // Employee's first name

    [Required, StringLength(50)]
    public string lastName { get; set; }                    // Employee's last name

    [Required]
    public DateTime birthday { get; set; }               // Employee's date of birth

    [Required, EmailAddress, StringLength(100)]
    public string email { get; set; }                        // Employee's email address (unique)

    // [Required, StringLength(50)]
    // public string DriverLicenseNumber { get; set; }          // Employee's driver's license number (unique)
    
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();    // Navigation properties
}