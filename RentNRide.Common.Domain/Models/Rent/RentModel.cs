using Newtonsoft.Json;
using System.Text.Json;

namespace RentNRide.Common.Domain.Models.Rent;

public class RentModel
{
    [JsonProperty("PlanoId")]
    public int PlanId { get; set; }
    [JsonProperty("EntregadorId")]
    public string DriverId { get; set; } = null!;
    [JsonProperty("MotoId")]
    public string MotorcycleId { get; set; } = null!;
}
