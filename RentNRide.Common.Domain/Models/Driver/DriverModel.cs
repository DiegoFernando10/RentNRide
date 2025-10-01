using Newtonsoft.Json;
using RentNRide.Common.Domain.Enums;

namespace RentNRide.Common.Domain.Models.Driver;

public class DriverModel
{
    [JsonProperty("EntregadorId")]
    public string DriverId { get; set; } = null!;
    [JsonProperty("Nome")]
    public string Name { get; set; } = null!;
    [JsonProperty("Cnpj")]
    public string Cnpj { get; set; } = null!;
    [JsonProperty("DataNascimento")]
    public DateOnly BirthDate { get; set; }
    [JsonProperty("NumeroCnh")]
    public string LicenseNumber { get; set; } = null!;
    [JsonProperty("TipoCnh")]
    public LicenseTypeEnum LicenseType { get; set; }
    [JsonProperty("UrlImagemChn")]
    public string? LicenseImageUrl { get; set; }
}
