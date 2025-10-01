using RentNRide.Common.Domain.Models.Motorcycle;

namespace RentNRide.Common.Domain.Services;

public interface IMotorcycleService
{
    Task<IEnumerable<MotorcycleModel>> GetAllAsync(string? plate);
    Task<MotorcycleModel> GetById(string motorcycleId);
    Task<string> CreateAsync(MotorcycleCreateModel model);
    Task UpdateAsync(string motorcycleId, MotorcycleUpdateModel model);
    Task DeleteAsync(string motorcycleId);
}
