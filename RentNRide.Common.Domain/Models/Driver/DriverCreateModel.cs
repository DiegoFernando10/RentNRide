using RentNRide.Common.Domain.Enums;
using RentNRide.Common.Extensions;

namespace RentNRide.Common.Domain.Models.Driver;

public class DriverCreateModel
{
    public string Name { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string LicenseNumber { get; set; } = null!;
    public LicenseTypeEnum LicenseType { get; set; }
    public string? Base64Image { get; set; }

    public void FormatData()
    {
        Name = Name.Trim();
        LicenseNumber = LicenseNumber.Trim();
        
        Cnpj = Cnpj.GetNumbers();

        if (!string.IsNullOrEmpty(Base64Image))
            Base64Image = Base64Image.Trim();
    }
}
