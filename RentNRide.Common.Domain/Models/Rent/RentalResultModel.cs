using Newtonsoft.Json;

namespace RentNRide.Common.Domain.Models.Rent;

public class RentalResultModel
{
    [JsonProperty("locacaoIdId")]
    public string RentalId { get; set; }
    [JsonProperty("PlanoId")]
    public int PlanId { get; set; }
    [JsonProperty("Descricao")]
    public string Description { get; set; } = null!;
    [JsonProperty("DataInicio")]
    public DateTime StartDate { get; set; }
    [JsonProperty("DataPrevisaoFim")]
    public DateTime ExpectedEndDate { get; set; }
    [JsonProperty("DataFim")]
    public DateTime? ActualReturnDate { get; set; }
    [JsonProperty("ValorBase")]
    public decimal BaseValue { get; set; }
    [JsonProperty("ValorTotal")]
    public decimal TotalCost { get; set; }
    [JsonProperty("Penalidade")]
    public decimal? Penalty { get; set; }
    [JsonProperty("Adicional")]
    public decimal? Additional { get; set; }
}
