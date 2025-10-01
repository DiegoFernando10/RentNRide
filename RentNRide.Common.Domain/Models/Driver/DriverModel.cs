using RentNRide.Common.Domain.Enums;

namespace RentNRide.Common.Domain.Models.Driver;

public class DriverModel
{
    public string DriverId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string LicenseNumber { get; set; } = null!;
    public LicenseTypeEnum LicenseType { get; set; }
    public string? LicenseImageUrl { get; set; }
}
