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
        .WithMessage("Modelo é obrigatório")
        .MaximumLength(50)
        .WithMessage("Modelo não pode ter mais de 50 caracteres");

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Placa é obrigatória")
            .MaximumLength(7)
            .WithMessage("Modelo não pode ter mais de 7 caracteres")
            .MinimumLength(7)
            .WithMessage("Modelo não pode ter menos de 7 caracteres");
    }
}