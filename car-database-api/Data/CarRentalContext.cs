using car_database_api.Models;
using Microsoft.EntityFrameworkCore;

namespace car_database_api.Data;

public class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
    {
    }

    // Define DbSet properties for each of the tables
    public DbSet<Car> Cars { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<CustomerApi> CustomerApis { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    // public DbSet<ReturnRecord> ReturnRecords { get; set; }
    public DbSet<RentalOffer> RentalOffers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasMany(c => c.Rentals)
            .WithOne(r => r.Car)
            .HasForeignKey(r => r.carId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Rentals)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.userId);

        // modelBuilder.Entity<Rental>()
        //     .HasOne(r => r.ReturnRecord)
        //     .WithOne(rr => rr.Rental)
        //     .HasForeignKey<ReturnRecord>(rr => rr.RentalId);

        modelBuilder.Entity<RentalOffer>()
            .HasOne(ro => ro.Car)
            .WithMany()
            .HasForeignKey(ro => ro.carId);
        
        modelBuilder.Entity<RentalOffer>()
            .HasOne(ro => ro.User)
            .WithMany()
            .HasForeignKey(ro => ro.userId);
    }
}