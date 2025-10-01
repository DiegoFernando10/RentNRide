using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Motorcycle;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

/// <summary>
/// API para gerenciamento de motos disponíveis para locação.
/// </summary>
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/motos")]
[ApiController]
public class MotorcycleController : ControllerBase
{
    private readonly IMotorcycleService motoService;
    private readonly IMotorcycleRegisteredService registeredService;

    public MotorcycleController(IMotorcycleService motoService, IMotorcycleRegisteredService registeredService)
    {
        this.motoService = motoService;
        this.registeredService = registeredService;
    }

    /// <summary>
    /// Lista todas as motos cadastradas, com opção de filtrar por placa.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MotorcycleModel>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<MotorcycleModel>> GetAll([FromQuery] string plate)
        => await motoService.GetAllAsync(plate);

    /// <summary>
    /// Retorna uma moto pelo identificador.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotorcycleModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<MotorcycleModel> GetById(string id)
        => await motoService.GetById(id);

    /// <summary>
    /// Cadastra uma nova moto.
    /// </summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> Create([FromBody] MotorcycleCreateModel model)
        => await motoService.CreateAsync(model);

    /// <summary>
    /// Atualiza os dados de uma moto existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Update(string id, [FromBody] MotorcycleUpdateModel model)
        => await motoService.UpdateAsync(id, model);

    /// <summary>
    /// Remove uma moto do sistema.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Delete(string id)
        => await motoService.DeleteAsync(id);

    /// <summary>
    /// Lista as motos já registradas em contratos de locação.
    /// </summary>
    [HttpGet("registered")]
    [ProducesResponseType(typeof(IEnumerable<MotorcycleModel>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<MotorcycleModel>> GetRegistereds([FromQuery] string plate)
        => await registeredService.GetAllAsync(plate);
}
