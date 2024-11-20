using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_database_api.Models;

public class Rental
    {
        [Key]
        public int id { get; set; }                    // Unique identifier for each rental

        [ForeignKey("Car")]
        public int carId { get; set; }                       // Foreign key reference to the Cars table

        [ForeignKey("Customer")]
        public int userId { get; set; }                  // Foreign key reference to the Customers table

        [Required]
        public DateTime startDate { get; set; }              // The start date and time of the rental

        public DateTime endDate { get; set; }               // The end date and time of the rental (nullable if rental is in progress)

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal totalPrice { get; set; }              // The total price for the rental

        [Required, StringLength(20)]
        [EnumDataType(typeof(RentalStatus))]
        public RentalStatus status { get; set; } = RentalStatus.planned;      // Rental status (default: Created)

        [StringLength(100)] public string startLocation { get; set; } = "Plac Politechnik, Warszawa";           // The location where the car is picked up

        [StringLength(100)] public string endLocation { get; set; } = "Plac Politechniki, Warszawa";          // The location where the car is dropped off (nullable)

        // public int? MileageAtStart { get; set; }             // Mileage of the car at the start of the rental

        // public int? MileageAtEnd { get; set; }               // Mileage of the car at the end of the rental (nullable if rental is in progress)
        
        // public ReturnRecord? ReturnRecord { get; set; }     // Return record associated with the rental (nullable)

        // Navigation properties
        public Car Car { get; set; }
        public User User { get; set; }
        public Employee Employee { get; set; }
    }