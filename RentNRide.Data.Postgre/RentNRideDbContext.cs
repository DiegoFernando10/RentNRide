using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentNRide.Data.Entities.Driver;
using RentNRide.Data.Entities.Motocycle;
using RentNRide.Data.Entities.Rent;

namespace RentNRide.Data.Postgre;

public class RentNRideDbContext : DbContext
{
    private readonly IConfiguration configuration;
    public RentNRideDbContext(DbContextOptions<RentNRideDbContext> options, IConfiguration configuration) : base(options)
    {
        configuration = configuration;
    }

    public DbSet<Motorcycle> Motorcicle { get; set; }
    public DbSet<Driver> Driver { get; set; }
    public DbSet<Rental> Rental { get; set; }
    public DbSet<MotorcycleEvent> MotorcycleEvent { get; set; }
    public DbSet<Plan> Plan { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Motorcycle
        modelBuilder.Entity<Motorcycle>()
            .ToTable("Motorcycle")
            .HasIndex(m => m.Plate)
            .IsUnique();

        // Driver
        modelBuilder.Entity<Driver>()
            .ToTable("Driver")
            .HasIndex(d => d.Cnpj)
            .IsUnique();

        modelBuilder.Entity<Driver>()
            .ToTable("Driver")
            .HasIndex(d => d.LicenseNumber)
            .IsUnique();

        // Rental
        modelBuilder.Entity<Rental>()
            .ToTable("Rental")
            .HasOne(r => r.Motorcycle)
            .WithMany(m => m.Rentals)
            .HasForeignKey(r => r.MotorcycleId);

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Driver)
            .WithMany()
            .HasForeignKey(r => r.DriverId);

        // MotorcycleEvent
        modelBuilder.Entity<MotorcycleEvent>()
            .ToTable("MotorcycleEvent")
            .HasOne(e => e.Motorcycle)
            .WithMany()
            .HasForeignKey(e => e.MotorcycleId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UsePostgresFromEnv(configuration);
        }
    }
}
