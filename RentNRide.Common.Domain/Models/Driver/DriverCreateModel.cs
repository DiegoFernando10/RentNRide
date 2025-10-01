using Newtonsoft.Json;
using RentNRide.Common.Domain.Enums;
using RentNRide.Common.Extensions;

namespace RentNRide.Common.Domain.Models.Driver;

public class DriverCreateModel
{
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
    [JsonProperty("ImagemCnhBase64")]
    public string? Base64Image { get; set; }

    public void FormatData()
    {
        Name = Name.Trim();
        LicenseNumber = LicenseNumber.Trim();
        
        Cnpj = Cnpj.GetNumbers();

        if (!string.IsNullOrEmpty(Base64Image))
            Base64Image = Base64Image.Trim();
    }
}
