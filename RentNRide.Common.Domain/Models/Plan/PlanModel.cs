using Newtonsoft.Json;

namespace RentNRide.Common.Domain.Models.Plan;

public class PlanModel
{
    [JsonProperty("PlanoId")]
    public int PlanId { get; set; }
    [JsonProperty("Descricao")]
    public string Description { get; set; } = null!;
    [JsonProperty("DuracaoEmDias")]
    public int DurationInDays { get; set; }
    [JsonProperty("ValorDiaria")]
    public decimal DailyValue { get; set; }
    [JsonProperty("PorcentagemMulta")]
    public decimal? FinePercentage { get; set; }
    [JsonProperty("ValorAdicionalDiaria")]
    public decimal? AditionalDailyValue { get; set; }
}
