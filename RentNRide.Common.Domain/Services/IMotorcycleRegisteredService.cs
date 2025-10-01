using RentNRide.Common.Domain.Models.Motorcycle;

namespace RentNRide.Common.Domain.Services;

public interface IMotorcycleRegisteredService
{
    Task<IEnumerable<MotorcycleModel>> GetAllAsync(string? plate);
}
