using FluentValidation;
using RentNRide.Common.Domain.Models.Motorcycle;

namespace RentNRide.Service.Motorcycle.Validators;
internal class UpdateValidator : AbstractValidator<MotorcycleUpdateModel>
{
    public UpdateValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Placa é obrigatória");
    }
}