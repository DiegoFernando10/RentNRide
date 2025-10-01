using Microsoft.EntityFrameworkCore;
using RentNRide.Common.Domain.Models.Motorcycle;
using RentNRide.Common.Domain.Services;
using RentNRide.Data.Entities;

namespace RentNRide.Service.Motorcycle;

public class MotorcycleRegisteredService : IMotorcycleRegisteredService
{
    private readonly IUnitOfWork unitOfWork;

    public MotorcycleRegisteredService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MotorcycleModel>> GetAllAsync(string? plate)
    {
        return await unitOfWork
                     .MotorcycleEventRepository
                     .Where(w => string.IsNullOrEmpty(plate) || w.Motorcycle.Plate.Contains(plate))
                     .Select(s => new MotorcycleModel
                     {
                         MotocycleId = s.MotorcycleId,
                         Plate = s.Motorcycle.Plate,
                         Model = s.Motorcycle.Model,
                         Year = s.Motorcycle.Year
                     })
                     .ToListAsync();
    }
}
