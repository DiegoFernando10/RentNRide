using RentNRide.Common.Domain.Interfaces;
using RentNRide.Data.Entities;
using RentNRide.Data.Entities.Driver;
using RentNRide.Data.Entities.Motocycle;
using RentNRide.Data.Entities.Rent;

namespace RentNRide.Data.PostgreSql;

public class UnitOfWork : IUnitOfWork
{
    private readonly RentNRideDbContext context;

    public UnitOfWork(RentNRideDbContext context)
    {
        this.context = context;
    }

    public IGenericRepository<Motorcycle> motorcycleRepository;
    public IGenericRepository<Motorcycle> MotorcycleRepository
    {
        get
        {
            return this.motorcycleRepository ??= new GenericRepository<RentNRideDbContext, Motorcycle>(context);
        }
    }


    public IGenericRepository<Driver> driverRepository;
    public IGenericRepository<Driver> DriverRepository
    {
        get
        {
            return this.driverRepository ??= new GenericRepository<RentNRideDbContext, Driver>(context);
        }
    }

    public IGenericRepository<Rental> rentalRepository;
    public IGenericRepository<Rental> RentalRepository
    {
        get
        {
            return this.rentalRepository ??= new GenericRepository<RentNRideDbContext, Rental>(context);
        }
    }

    public IGenericRepository<MotorcycleEvent> motorcycleEventRepository;
    public IGenericRepository<MotorcycleEvent> MotorcycleEventRepository
    {
        get
        {
            return this.motorcycleEventRepository ??= new GenericRepository<RentNRideDbContext, MotorcycleEvent>(context);
        }
    }

    public IGenericRepository<Plan> planRepository;
    public IGenericRepository<Plan> PlanRepository
    {
        get
        {
            return this.planRepository ??= new GenericRepository<RentNRideDbContext, Plan>(context);
        }
    }

    public async Task SaveChangesAsync()
    {
        await this.context.SaveChangesAsync();
    }
}
