using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/locacao")]
public class RentalController
{
    private readonly IRentalService rentalService;

    public RentalController(IRentalService rentalService)
    {
        this.rentalService = rentalService;
    }

}
