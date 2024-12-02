using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Coefficients.Services;
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
    [HttpGet("/Event/{eventId}/Coefficient")]
    public async Task<IActionResult> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var result = await service.GetByEventIdAsync(eventId, cancellationToken);
        return Ok(result);
    }
}