using FluentValidation;
using RentNRide.Common.Domain.Models.Motorcycle;

namespace RentNRide.Service.Motorcycle.Validators;
internal class CreateValidator : AbstractValidator<MotorcycleCreateModel>
{
    public CreateValidator()
    {
        RuleFor(x => x.Year)
        .GreaterThanOrEqualTo(1900)
        .LessThanOrEqualTo(DateTime.Now.Year + 1)
        .WithMessage($"O ano deve ser entre 1900 e {DateTime.Now.Year + 1}");

        RuleFor(x => x.Model)
        .NotEmpty()
        .WithMessage("Modelo é obrigatório");

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Placa é obrigatória");
    }
}