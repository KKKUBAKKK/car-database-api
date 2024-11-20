using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace car_database_api.Models;

public class RentalOffer
{
    [Key]
    public int id { get; set; }
    public int carId { get; set; }
    public int userId { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal dailyRate { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal insuranceRate { get; set; }
    public DateTime validUntil { get; set; }
    public bool isActive { get; set; }
    
    // Navigation property
    public Car Car { get; set; }
    public User User { get; set; }
}