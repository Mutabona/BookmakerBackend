using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Teams.Services;
using BookmakerBackend.Contracts.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Команды.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class TeamController(ITeamService service) : BaseController
{
    /// <summary>
    /// Добавляет команду по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Worker")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddAsync(AddTeamRequest request, CancellationToken token)
    {
        await service.AddAsync(request, token);
        return Created();
    }

    /// <summary>
    /// Обновляет команду по модели запроса.
    /// </summary>
    /// <param name="id">Идентификатор команды.</param>
    /// <param name="request">Запрос.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Worker")]
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateTeamRequest request, CancellationToken token)
    {
        await service.UpdateAsync(id, request, token);
        return Ok();
    }

    /// <summary>
    /// Удаляет команду по идентификатору..
    /// </summary>
    /// <param name="id">Идентификатор команды.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Worker")]
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
    {
        await service.DeleteAsync(id, token);
        return NoContent();
    }

    /// <summary>
    /// Получает команду по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Модель команды.</returns>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeamDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken token)
    {
        var team = await service.GetByIdAsync(id, token);
        return Ok(team);
    }

    /// <summary>
    /// Получает команды по строке поиска.
    /// </summary>
    /// <param name="searchString">Строка поиска.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Коллекция моделей команд.</returns>
    [Authorize]
    [HttpGet("Search/{searchString}")]
    [ProducesResponseType(typeof(TeamDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SearchAsync(string searchString, CancellationToken token)
    {
        var teams = await service.SearchAsync(searchString, token);
        return Ok(teams);
    }
}