using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_database_api.Models;

public class CustomerApi
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }                    // Unique identifier for each employee
    
    [Required, StringLength(10)]
    public string username { get; set; }                   // CustomerApi's username (unique)
    
    [Required, StringLength(20)]
    public string password { get; set; }                   // CustomerApi's password
}