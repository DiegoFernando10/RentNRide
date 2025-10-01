namespace RentNRide.Data.Entities.Motocycle;

public class MotorcycleEvent
{
    public string MotorcycleEventId { get; set; } = null!;
    public DateTime CreatedDatetime { get; set; }

    public string MotorcycleId { get; set; } = null!;
    public Motorcycle Motorcycle { get; set; } = null!;
}
