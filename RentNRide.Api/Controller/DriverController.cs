using Microsoft.AspNetCore.Mvc;
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
}
