using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Bets.Services;
using BookmakerBackend.Contracts.Bets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Ставки.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class BetController(IBetService service) : BaseController
{
    /// <summary>
    /// Создаёт ставку пользователя по модели запроса на коэффициент.
    /// </summary>
    /// <param name="coefficientId">Идентификатор коэффициента.</param>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddAsync(Guid coefficientId, AddBetRequest request, CancellationToken cancellationToken)
    {
        var username = GetCurrentUserUsername();
        await service.AddAsync(username, coefficientId, request, cancellationToken);
        return Created();
    }

    /// <summary>
    /// Удаляет ставку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Получает ставку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель ставки.</returns>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BetDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bet = await service.GetByIdAsync(id, cancellationToken);
        return Ok(bet);
    }

    /// <summary>
    /// Получает ставки пользователя.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей ставок.</returns>
    [Authorize]
    [HttpGet("User/{userId}/Bet")]
    [ProducesResponseType(typeof(ICollection<BetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByUsernameAsync(CancellationToken cancellationToken)
    {
        var username = GetCurrentUserUsername();
        var bets = await service.GetByUsernameAsync(username, cancellationToken);
        return Ok(bets);
    }
}