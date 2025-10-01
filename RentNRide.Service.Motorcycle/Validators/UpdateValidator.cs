using FluentValidation;
using RentNRide.Common.Domain.Models.Motorcycle;

namespace RentNRide.Service.Motorcycle.Validators;
internal class UpdateValidator : AbstractValidator<MotorcycleUpdateModel>
{
    public UpdateValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Placa é obrigatória")
            .MaximumLength(7)
            .WithMessage("Modelo não pode ter mais de 7 caracteres")
            .MinimumLength(7)
            .WithMessage("Modelo não pode ter menos de 7 caracteres");
    }
}