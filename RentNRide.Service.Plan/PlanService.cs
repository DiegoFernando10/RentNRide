using Microsoft.EntityFrameworkCore;
using RentNRide.Common.Domain.Exceptions;
using RentNRide.Common.Domain.Models.Driver;
using RentNRide.Common.Domain.Models.Plan;
using RentNRide.Common.Domain.Services;
using RentNRide.Data.Entities;

namespace RentNRide.Service.Plan;

public class PlanService : IPlanService
{
    private readonly IUnitOfWork unitOfWork;

    public PlanService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PlanModel>> GetAllAsync()
    {
        return await unitOfWork
            .PlanRepository
            .AsQueryable()
            .Select(s => new PlanModel
            {
                PlanId = s.PlanId,
                Description = s.Description,
                DurationInDays = s.DurationInDays,
                DailyValue = s.DailyValue,
                FinePercentage = s.FinePercentage,
                AditionalDailyValue = s.AditionalDailyValue
            }).ToListAsync();
    }
}
