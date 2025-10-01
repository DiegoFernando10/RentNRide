using Newtonsoft.Json;

namespace RentNRide.Common.Domain.Models.Driver;

public class UploadLicenseModel
{
    [JsonProperty("ImagemCnhBase64")]
    public string Base64Image { get; set; } = null!;
}
