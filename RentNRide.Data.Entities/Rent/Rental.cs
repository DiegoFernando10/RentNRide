namespace RentNRide.Data.Entities.Rent;

public class Rental
{
    public string RentalId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal BaseValue { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Additional { get; set; }
    public decimal TotalCost { get; set; }

    // Motorcycle
    public string MotorcycleId { get; set; } = null!;
    public Motocycle.Motorcycle Motorcycle { get; set; } = null!;

    // Driver
    public string DriverId { get; set; } = null!;
    public Driver.Driver Driver { get; set; } = null!;
    
    // Plans
    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;
}
