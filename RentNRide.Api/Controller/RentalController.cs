using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Driver;
using RentNRide.Common.Domain.Models.Rent;
using RentNRide.Common.Domain.Services;
using RentNRide.Service.Driver;

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

    [HttpGet("{id}")]
    public async Task<RentalResultModel> Get(string id)
    {
        return await rentalService.GetById(id);
    }

    [HttpPost]
    public async Task<RentalResultModel> Create([FromBody] RentModel model)
    {
        return await rentalService.CreateAsync(model);
    }

    [HttpPut("{id}/devolucao")]
    public async Task UpdateCnh(string id, [FromBody] DateFinishModel model)
    {
        await rentalService.FinishRentalAsync(id, model.FinishDate);
    }

}
