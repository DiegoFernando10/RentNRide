using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Driver;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

/// <summary>
/// API para gerenciamento de entregadores.
/// </summary>
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/entregadores")]
[ApiController]
public class DriverController : ControllerBase
{
    private readonly IDriverService driverService;

    public DriverController(IDriverService driverService)
    {
        this.driverService = driverService;
    }

    /// <summary>
    /// Lista todos os entregadores cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DriverModel>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<DriverModel>> GetAll()
        => await driverService.GetAll();

    /// <summary>
    /// Cadastra um novo entregador.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> Create([FromBody] DriverCreateModel model)
        => await driverService.CreateAsync(model);

    /// <summary>
    /// Atualiza a CNH de um entregador.
    /// </summary>
    [HttpPost("{id}/cnh")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateCnh(string id, [FromBody] UploadLicenseModel model)
        => await driverService.UpdateDriveLicenseAsync(id, model);
}
