using RentNRide.Data.Entities.Rent;

namespace RentNRide.Data.Entities.Motocycle;

public class Motorcycle
{
    public string MotorcycleId { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string Plate { get; set; } = null!;

    public ICollection<Rental> Rentals { get; set; } = [];
}
