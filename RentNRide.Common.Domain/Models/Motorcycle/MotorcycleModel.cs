using Newtonsoft.Json;

namespace RentNRide.Common.Domain.Models.Motorcycle;

public class MotorcycleModel
{
    [JsonProperty("MotoId")]
    public string MotocycleId { get; set; } = null!;
    [JsonProperty("Ano")]
    public int Year { get; set; }
    [JsonProperty("Modelo")]
    public string Model { get; set; } = null!;
    [JsonProperty("Placa")]
    public string Plate { get; set; } = null!;
}
