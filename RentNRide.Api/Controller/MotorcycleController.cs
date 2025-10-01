using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Motorcycle;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/motos")]
public class MotorcycleController
{
    private readonly IMotorcycleService motoService;

    public MotorcycleController(IMotorcycleService motoService)
    {
        this.motoService = motoService;
    }

    [HttpGet()]
    public async Task<IEnumerable<MotorcycleModel>> GetAll([FromQuery] string plate)
    {
        return await motoService.GetAllAsync(plate);
    }

    [HttpGet("{id}")]
    public async Task<MotorcycleModel> GetById(string id)
    {
        return await motoService.GetById(id);
    }

    [HttpPost]
    public async Task<string> Create([FromBody] MotorcycleCreateModel model)
    {
        return await motoService.CreateAsync(model);
    }

    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] MotorcycleUpdateModel model)
    {
        await motoService.UpdateAsync(id, model);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await motoService.DeleteAsync(id);
    }
}
