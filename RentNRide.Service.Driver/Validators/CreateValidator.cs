using FluentValidation;
using RentNRide.Common.Domain.Models.Driver;

namespace RentNRide.Service.Motorcycle.Validators;
internal class CreateValidator : AbstractValidator<DriverCreateModel>
{
    public CreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("Nome não pode passar de 100 caracteres.");

        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("CNPJ é obrigatório.")
            .Matches(@"^\d{14}$")
            .WithMessage("CNPJ deve conter apenas 14 números.");

        RuleFor(x => x.BirthDate)
           .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
           .LessThan(DateOnly.FromDateTime(DateTime.Today))
           .WithMessage("Data de nascimento é inválida.")
           .Must(AtLeast18YearsOld)
           .WithMessage("O entregador deve ter no mínimo 18 anos.");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty()
            .WithMessage("Número da CNH é obrigatório.")
            .MaximumLength(20);

        RuleFor(x => x.LicenseType)
            .IsInEnum()
            .WithMessage("Tipo de CNH inválido (válido: A, B ou A+B).");


        RuleFor(x => x.Base64Image)
            .Must(IsValidBase64)
            .WithMessage("Imagem em Base64 inválida.")
            .Must(HasValidFormat)
            .WithMessage("Apenas PNG ou BMP são aceitos.")
            .Must(NotExceedMaxSize)
            .WithMessage("A imagem não pode ultrapassar 5MB.")
            .When(x => !string.IsNullOrEmpty(x.Base64Image));
    }

    private bool AtLeast18YearsOld(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age >= 18;
    }

    private bool IsValidBase64(string base64)
    {
        try
        {
            var base64Data = base64.Contains(",") ? base64.Split(',')[1] : base64;
            Convert.FromBase64String(base64Data);
            return true;
        }
        catch { return false; }
    }

    private bool HasValidFormat(string base64)
    {
        return base64.StartsWith("data:image/png;base64,") || base64.StartsWith("data:image/bmp;base64,");
    }

    private bool NotExceedMaxSize(string base64)
    {
        try
        {
            var base64Data = base64.Contains(',') ? base64.Split(',')[1] : base64;
            var bytes = Convert.FromBase64String(base64Data);
            return bytes.Length <= (5 * 1024 * 1024); // 5 MB
        }
        catch { return false; }
    }
}