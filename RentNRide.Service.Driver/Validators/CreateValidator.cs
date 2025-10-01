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
            .WithMessage("CNPJ deve conter 14 números.")
            .Must(IsValidCnpj)
            .WithMessage("CNPJ inválido.");

        RuleFor(x => x.BirthDate)
           .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
           .LessThan(DateOnly.FromDateTime(DateTime.Today))
           .WithMessage("Data de nascimento é inválida.")
           .Must(AtLeast18YearsOld)
           .WithMessage("O entregador deve ter no mínimo 18 anos.");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty()
            .WithMessage("Número da CNH é obrigatório.")
            .MaximumLength(11)
            .WithMessage("Número da CNH deve ter no máximo 11 dígitos.")
            .MinimumLength(11)
            .WithMessage("Número da CNH deve ter no mínimo 11 dígitos.");

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

    private bool IsValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;

        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14) return false;

        if (cnpj.All(c => c == cnpj[0])) return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCnpj += digito;
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito += resto.ToString();

        return cnpj.EndsWith(digito);
    }

}