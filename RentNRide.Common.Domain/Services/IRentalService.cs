using RentNRide.Common.Domain.Models.Rent;

namespace RentNRide.Common.Domain.Services;

public interface IRentalService
{
    Task<IEnumerable<RentalResultModel>> GetAll();
    Task<RentalResultModel> CreateRentalAsync(RentModel model);
    Task<RentalResultModel> FinishRentalAsync(string rentalId, DateTime finishDate);

}
