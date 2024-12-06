using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Events.Services;
using BookmakerBackend.Contracts.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// События.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class EventController(IEventService service) : BaseController
{
    /// <summary>
    /// Создаёт событие по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Worker")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddAsync(AddEventRequest request, CancellationToken token)
    {
        await service.AddAsync(request, token);
        return Created();
    }

    /// <summary>
    /// Изменяет событие по модели запроса.
    /// </summary>
    /// <param name="id">Идентификатор события..</param>
    /// <param name="request">Запрос.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Worker")]
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateEventRequest request, CancellationToken token)
    {
        await service.UpdateAsync(id, request, token);
        return Ok();
    }

    /// <summary>
    /// Получает событие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор события.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Модель события.</returns>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EventDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken token)
    {
        var eventDto = await service.GetByIdAsync(id, token);
        return Ok(eventDto);
    }

    
    /// <summary>
    /// Выполняет поиск событий по имени.
    /// </summary>
    /// <param name="searchString">Строка поиска.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Коллекция моделей событий.</returns>
    [Authorize]
    [HttpGet("Search/{searchString}")]
    [ProducesResponseType(typeof(ICollection<EventDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SearchAsync(string searchString, CancellationToken token)
    {
        var events = await service.SearchAsync(searchString, token);
        return Ok(events);
    }
}