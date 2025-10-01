using Microsoft.EntityFrameworkCore;
using RentNRide.Common.Domain.Exceptions;
using RentNRide.Common.Domain.Interfaces;
using RentNRide.Common.Domain.Models.Motorcycle;
using RentNRide.Common.Domain.Services;
using RentNRide.Common.Id;
using RentNRide.Common.Validation;
using RentNRide.Data.Entities;
using RentNRide.Data.Entities.Motocycle;
using RentNRide.Service.Motorcycle.Validators;

namespace RentNRide.Service.Motorcycle;

public class MotorcycleService : IMotorcycleService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMessagePublisher publisher;

    public MotorcycleService(IUnitOfWork unitOfWork, IMessagePublisher publisher)
    {
        this.unitOfWork = unitOfWork;
        this.publisher = publisher;
    }

    public async Task<IEnumerable<MotorcycleModel>> GetAllAsync(string? plate)
    {
        return await unitOfWork
                     .MotorcycleRepository
                     .Where(w => string.IsNullOrEmpty(plate) || w.Plate.Contains(plate))
                     .Select(s => new MotorcycleModel
                     {
                         MotocycleId = s.MotorcycleId,
                         Plate = s.Plate,
                         Model = s.Model,
                         Year = s.Year
                     })
                     .ToListAsync();
    }

    public async Task<MotorcycleModel> GetById(string motorcycleId)
    {
        return await unitOfWork
                 .MotorcycleRepository
                 .Where(m => m.MotorcycleId == motorcycleId)
                 .Select(s => new MotorcycleModel
                 {
                     MotocycleId = s.MotorcycleId,
                     Plate = s.Plate,
                     Model = s.Model,
                     Year = s.Year
                 })
                 .FirstOrDefaultAsync() ?? throw new NotFoundException("Moto não localizada");
    }

    public async Task<string> CreateAsync(MotorcycleCreateModel model)
    {
        #region Validations

        model.FormatData();

        await new CreateValidator().Run(model);

        if (await unitOfWork.MotorcycleRepository.AnyAsync(m => m.Plate == model.Plate))
            throw new ApiException("A Placa informada já está vinculada a outra moto.");

        #endregion

        var newId = await IdGenerator.NewSync(6);

        var motorcycle = new Data.Entities.Motocycle.Motorcycle()
        {
            MotorcycleId = newId,
            Plate = model.Plate,
            Model = model.Model,
            Year = model.Year
        };

        unitOfWork.MotorcycleRepository.Add(motorcycle);

        await unitOfWork.SaveChangesAsync();

        // Evnt when year 2024
        if (model.Year == 2024)
        {
            await publisher.PublishAsync("motorcycle.created", motorcycle);
        }
        return newId;
    }

    public async Task UpdateAsync(string motorcycleId, MotorcycleUpdateModel model)
    {
        var motorcycle = await unitOfWork
            .MotorcycleRepository
            .FirstOrDefaultAsync(m => m.MotorcycleId == motorcycleId) ?? throw new NotFoundException("Moto não localizada");

        #region Validations

        model.FormatData();

        await new UpdateValidator().Run(model);

        if (motorcycle.Plate == model.Plate)
            throw new ApiException("A Placa informada já está vinculada a esta moto.");

        if (await unitOfWork.MotorcycleRepository.AnyAsync(m => m.Plate == model.Plate && m.MotorcycleId != motorcycleId))
            throw new ApiException("A Placa informada já está vinculada a outra moto.");

        #endregion

        motorcycle.Plate = model.Plate;

        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string motorcycleId)
    {
        var motorcycle = await unitOfWork
           .MotorcycleRepository
           .AsQueryable()
           .Include(i => i.Rentals)
           .FirstOrDefaultAsync(m => m.MotorcycleId == motorcycleId) ?? throw new NotFoundException("Moto não localizada");

        if (motorcycle.Rentals.Count > 0)
            throw new ApiException("Não é possível remover uma moto com locações.");
    }
}
