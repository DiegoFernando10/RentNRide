using Newtonsoft.Json;

namespace RentNRide.Common.Domain.Models.Rent;

public class DateFinishModel
{
    [JsonProperty("DataFinalizacao")]
    public DateTime FinishDate { get; set; }
}
