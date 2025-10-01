using RentNRide.Common.Domain.Interfaces;
using RentNRide.Data.Entities.Motocycle;
using RentNRide.Data.Entities.Rent;

namespace RentNRide.Data.Entities;
public interface IUnitOfWork
{
    IGenericRepository<Motorcycle> MotorcycleRepository { get; }
    IGenericRepository<Driver.Driver> DriverRepository { get; }
    IGenericRepository<Rental> RentalRepository { get; }
    IGenericRepository<MotorcycleEvent> MotorcycleEventRepository { get; }
    IGenericRepository<Plan> PlanRepository { get; }
    Task SaveChangesAsync();
}
