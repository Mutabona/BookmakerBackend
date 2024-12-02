using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Coefficients.Services;
using BookmakerBackend.Contracts.Coefficients;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Коэффициенты.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class CoefficientController(ICoefficientService service) : BaseController
{
    /// <summary>
    /// Получает коэффициенты по идентификатору события.
    /// </summary>
    /// <param name="eventId">Идентификатор события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция коэффициентов.</returns>
    [HttpGet("/Event/{eventId}/Coefficient")]
    [ProducesResponseType(typeof(ICollection<CoefficientDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var result = await service.GetByEventIdAsync(eventId, cancellationToken);
        return Ok(result);
    }
}