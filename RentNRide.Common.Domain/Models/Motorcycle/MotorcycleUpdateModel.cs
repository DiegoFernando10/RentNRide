using System.Text.RegularExpressions;

namespace RentNRide.Common.Domain.Models.Motorcycle
{
    public class MotorcycleUpdateModel
    {
        public string Plate { get; set; } = null!;

        public void FormatData()
        {
            // Remove extra spaces
            Plate = Plate.Trim();

            // Remove any character that is not a letter or a number
            Plate = Regex.Replace(Plate, "[^a-zA-Z0-9]", "");

            // Normalize to upper
            Plate = Plate.ToUpperInvariant();
        }
    }
}
