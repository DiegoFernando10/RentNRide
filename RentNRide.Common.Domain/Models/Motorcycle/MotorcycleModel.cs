using System.Text.RegularExpressions;

namespace RentNRide.Common.Domain.Models.Motorcycle
{
    public class MotorcycleModel
    {
        public string MotocycleId { get; set; } = null!;
        public int Year { get; set; }
        public string Model { get; set; } = null!;
        public string Plate { get; set; } = null!;
    }
}
