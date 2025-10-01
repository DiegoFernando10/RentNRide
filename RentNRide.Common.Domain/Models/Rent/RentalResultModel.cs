namespace RentNRide.Common.Domain.Models.Rent;

public class RentalResultModel
{
    public string RentalId { get; set; }
    public int PlanId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal BaseValue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Additional { get; set; }
}
