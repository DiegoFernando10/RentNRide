using RentNRide.Common.Domain.Models.Plan;

namespace RentNRide.Common.Domain.Services;

public interface IPlanService
{
    Task<IEnumerable<PlanModel>> GetAllAsync();

}
