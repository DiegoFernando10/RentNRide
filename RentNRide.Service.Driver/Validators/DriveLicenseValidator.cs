using FluentValidation;
using RentNRide.Common.Domain.Models.Driver;

namespace RentNRide.Service.Driver.Validators;

internal class DriveLicenseValidator : AbstractValidator<UploadLicenseModel>
{
    public DriveLicenseValidator()
    {
        RuleFor(x => x.Base64Image)
            .NotEmpty()
            .WithMessage("A imagem da CNH é obrigatória.")
            .Must(IsValidBase64)
            .WithMessage("Imagem em Base64 inválida.")
            .Must(HasValidFormat)
            .WithMessage("Apenas PNG ou BMP são aceitos.")
            .Must(NotExceedMaxSize)
            .WithMessage("A imagem não pode ultrapassar 5MB.");
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
