using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_database_api.Models;

public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }                      // Unique identifier for each car

    [Required, StringLength(50)]
    public string producer { get; set; }                 // Car company (e.g., Toyota, Ford)

    [Required, StringLength(50)]
    public string model { get; set; }                   // Car model (e.g., Corolla, Focus)

    [Required]
    public int yearOfProduction { get; set; }                       // Year of manufacture

    // public GearboxType Gearbox { get; set; } = GearboxType.Manual;             // Gearbox type (Automatic, Manual)

    // [Required, StringLength(20)]
    // [EnumDataType(typeof(FuelType))]
    // public FuelType FuelType { get; set; } = FuelType.Petrol;                 // Type of fuel (Petrol, Diesel, etc.)

    [Required, Range(1, int.MaxValue)]
    public int numberOfSeats { get; set; }                 // Number of passengers
    
    [Required]
    [EnumDataType(typeof(CarType))]
    public CarType type { get; set; } = CarType.economy;                // Car category (small, medium, large)

    // [StringLength(20)]
    // public string LicensePlate { get; set; }            // License plate number (unique)

    // public int Mileage { get; set; }                    // Mileage of the car (in km)

    // [Required]
    [EnumDataType(typeof(Boolean))]
    public Boolean isAvailable { get; set; } = true; // Availability status (default: available)

    [StringLength(100)] public string location { get; set; } = "Plac Politechniki, Warszawa";                // Location of the car (e.g., city, parking lot)
    
    // Navigation properties
    // public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}