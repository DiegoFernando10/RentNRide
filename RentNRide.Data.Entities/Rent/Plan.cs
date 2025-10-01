namespace RentNRide.Data.Entities.Rent;

public class Plan
{
    public int PlanId { get; set; }
    public string Description { get; set; } = null!;
    public int DurationInDays { get; set; }
    public decimal DailyValue { get; set; }
    public decimal? FinePercentage { get; set; }
    public decimal? AditionalDailyValue { get; set; }
}
