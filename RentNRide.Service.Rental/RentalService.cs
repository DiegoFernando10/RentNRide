using Microsoft.EntityFrameworkCore;
using RentNRide.Common.Domain.Enums;
using RentNRide.Common.Domain.Exceptions;
using RentNRide.Common.Domain.Models.Rent;
using RentNRide.Common.Domain.Services;
using RentNRide.Common.Id;
using RentNRide.Data.Entities;

namespace RentNRide.Service.Rental;

public class RentalService : IRentalService
{
    private readonly IUnitOfWork unitOfWork;

    public RentalService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<RentalResultModel> GetById(string rentalId)
    {
        return await unitOfWork
            .RentalRepository
            .Where(w => w.RentalId == rentalId)
            .Include(i => i.Plan)
            .Select(s => new RentalResultModel
            {
                RentalId = s.RentalId,
                ActualReturnDate = s.ActualEndDate,
                Additional = s.Additional,
                BaseValue = s.BaseValue,
                ExpectedEndDate = s.ExpectedEndDate,
                TotalCost = s.TotalCost,
                Penalty = s.Penalty,
                StartDate = s.StartDate,
                Description = s.Plan.Description,
                PlanId = s.PlanId
            }).FirstOrDefaultAsync() ?? throw new NotFoundException("Locação não encontrada");
    }

    public async Task<RentalResultModel> CreateAsync(RentModel model)
    {
   

        #region Validations

        var plan = await unitOfWork
            .PlanRepository
            .Where(p => p.PlanId == model.PlanId)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("Plano não encontrado.");

        var driver = await unitOfWork
            .DriverRepository
            .Where(d => d.DriverId == model.DriverId)
            .Select(s => new
            {
                s.LicenseType
            })
            .FirstOrDefaultAsync() ?? throw new NotFoundException("Entregador não encontrado.");

        if (driver.LicenseType != LicenseTypeEnum.A && driver.LicenseType != LicenseTypeEnum.AB)
            throw new ApiException("Somente entregadores habilitados na categoria A podem alugar motos.");

        if (!await unitOfWork.MotorcycleRepository.AnyAsync(d => d.MotorcycleId == model.MotorcycleId))
            throw new NotFoundException("Moto não encontrada.");

        var startDate = DateTime.UtcNow.AddHours(-3).AddDays(1);
        var expectedEndDate = startDate.AddDays(plan.DurationInDays);

        var overlappingRental = await unitOfWork
            .RentalRepository
            .AnyAsync(r => r.MotorcycleId == model.MotorcycleId &&
                           r.ActualEndDate == null &&
                           r.StartDate.Date <= startDate.Date &&
                           r.ExpectedEndDate.Date >= startDate.Date);

        if (overlappingRental)
            throw new ApiException("Moto já possui uma locação nesse período.");

        #endregion

        var baseValue = plan.DailyValue * plan.DurationInDays;

        var rental = new Data.Entities.Rent.Rental
        {
            RentalId = await IdGenerator.NewSync(6),
            DriverId = model.DriverId,
            MotorcycleId = model.MotorcycleId,
            PlanId = model.PlanId,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            BaseValue = plan.DailyValue * plan.DurationInDays,
            TotalCost = plan.DailyValue * plan.DurationInDays
        };

        unitOfWork.RentalRepository.Add(rental);

        await unitOfWork.SaveChangesAsync();

        return new RentalResultModel
        {
            RentalId = rental.RentalId,
            PlanId = plan.PlanId,
            Description = plan.Description,
            StartDate = startDate,
            ExpectedEndDate = expectedEndDate,
            BaseValue = baseValue
        };
    }

    public async Task<RentalResultModel> FinishRentalAsync(string rentalId, DateTime finishDate)
    {
        #region Validations

        var rental = await unitOfWork
            .RentalRepository
            .Where(w => w.RentalId == rentalId)
            .Include(r => r.Plan)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("Locação não encontrada");

        var plan = rental.Plan;

        if (rental.StartDate.Date >= finishDate.Date)
            throw new ApiException("A data de finalização não pode ser menor que data de início.");

        #endregion

        rental.ActualEndDate = finishDate;
        rental.Penalty = 0;
        rental.Additional = 0;

        // before expected date
        if (finishDate.Date < rental.ExpectedEndDate.Date)
        {
            var missingDays = (finishDate.Date - rental.StartDate.Date).Days;

            rental.Penalty = missingDays * (plan.FinePercentage ?? 0) / 100;
            rental.TotalCost = rental.BaseValue + rental.Penalty.Value;
        }
        // after expected date
        else if (finishDate.Date > rental.ExpectedEndDate.Date)
        {
            var extraDays = (finishDate.Date - rental.ExpectedEndDate).Days;
            rental.Additional = extraDays * (plan.AditionalDailyValue ?? 0);
            rental.TotalCost = rental.BaseValue + rental.Additional.Value;
        }
        // on date
        else
        {
            rental.TotalCost = rental.BaseValue;
        }

        await unitOfWork.SaveChangesAsync();

        return new RentalResultModel
        {
            RentalId = rental.RentalId,
            PlanId = plan.PlanId,
            StartDate = rental.StartDate,
            ExpectedEndDate = rental.ExpectedEndDate,
            ActualReturnDate = finishDate,
            TotalCost = rental.TotalCost,
            Penalty = rental.Penalty,
            Additional = rental.Additional
        };
    }
}
