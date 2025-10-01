using Microsoft.AspNetCore.Mvc;
using RentNRide.Common.Domain.Models.Rent;
using RentNRide.Common.Domain.Services;

namespace RentNRide.Api.Controller;

/// <summary>
/// API para gerenciamento de locações.
/// </summary>
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/locacao")]
[ApiController]
public class RentalController : ControllerBase
{
    private readonly IRentalService rentalService;

    public RentalController(IRentalService rentalService)
    {
        this.rentalService = rentalService;
    }

    /// <summary>
    /// Consulta uma locação pelo identificador.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RentalResultModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<RentalResultModel> Get(string id)
        => await rentalService.GetById(id);

    /// <summary>
    /// Cria uma nova locação.
    /// </summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RentalResultModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<RentalResultModel> Create([FromBody] RentModel model)
        => await rentalService.CreateAsync(model);

    /// <summary>
    /// Finaliza uma locação (devolução da moto).
    /// </summary>
    [HttpPut("{id}/devolucao")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<RentalResultModel> UpdateCnh(string id, [FromBody] DateFinishModel model)
        => await rentalService.FinishRentalAsync(id, model.FinishDate);
}
