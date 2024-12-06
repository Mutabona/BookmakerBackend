using BookmakerBackend.Contracts.Events;

namespace BookmakerBackend.AppServices.Contexts.Events.Services;

/// <summary>
/// Сервися для работы с событиями.
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Создаёт событие по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(AddEventRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет событие по модели запроса.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(Guid id, UpdateEventRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Выполняет поиск событий по назнванию.
    /// </summary>
    /// <param name="searchString">Строка поиска.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей событий.</returns>
    Task<ICollection<EventDto>> SearchAsync(string searchString, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает событие по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель события.</returns>
    Task<EventDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}