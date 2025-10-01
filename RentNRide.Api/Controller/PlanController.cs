using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Plan;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

/// <summary>
/// API para consulta de planos de locação.
/// </summary>
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/planos")]
[ApiController]
public class PlanController : ControllerBase
{
    private readonly IPlanService planService;

    public PlanController(IPlanService planService)
    {
        this.planService = planService;
    }

    /// <summary>
    /// Lista todos os planos de locação disponíveis.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlanModel>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<PlanModel>> GetAll()
        => await planService.GetAllAsync();
}
