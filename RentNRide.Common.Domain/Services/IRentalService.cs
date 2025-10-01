using RentNRide.Common.Domain.Models.Rent;

namespace RentNRide.Common.Domain.Services;

public interface IRentalService
{
    Task<RentalResultModel> GetById(string rentalId);
    Task<RentalResultModel> CreateAsync(RentModel model);
    Task<RentalResultModel> FinishRentalAsync(string rentalId, DateTime finishDate);

}
