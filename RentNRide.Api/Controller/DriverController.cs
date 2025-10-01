using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Driver;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/entregadores")]
public class DriverController
{
    private readonly IDriverService driverService;

    public DriverController(IDriverService driverService)
    {
        this.driverService = driverService;
    }

    [HttpPost]
    public async Task<string> Create([FromBody] DriverCreateModel model)
    {
        return await driverService.CreateAsync(model);
    }

    [HttpPost("{id}/cnh")]
    public async Task UpdateCnh(string id, [FromBody] UploadLicenseModel model)
    {
        await driverService.UpdateDriveLicenseAsync(id, model);
    }
}
