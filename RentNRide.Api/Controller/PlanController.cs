using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Plan;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/planos")]
public class PlanController
{
    private readonly IPlanService planService;

    public PlanController(IPlanService planService)
    {

        this.planService = planService;
    }

    [HttpGet]
    public async Task<IEnumerable<PlanModel>> GetAll()
    {
        return await planService.GetAllAsync();
    }
}
