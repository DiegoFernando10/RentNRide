using RentNRide.Common.Domain.Models.Driver;

namespace RentNRide.Common.Domain.Services;

public interface IDriverService
{
    Task<IEnumerable<DriverModel>> GetAll();
    Task<DriverModel> GetById(string driverId);
    Task<string> CreateAsync(DriverCreateModel driver);
    Task UpdateDriveLicenseAsync(string driverId, UploadLicenseModel model);
}
