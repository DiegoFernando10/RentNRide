namespace RentNRide.Common.Domain.Models.Rent;

public class RentModel
{
    public int PlanId { get; set; }
    public string DriverId { get; set; } = null!;
    public string MotorcycleId { get; set; } = null!;
}
