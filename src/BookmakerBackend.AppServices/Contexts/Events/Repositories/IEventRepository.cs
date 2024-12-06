using BookmakerBackend.Contracts.Events;

namespace BookmakerBackend.AppServices.Contexts.Events.Repositories;

/// <summary>
/// Репозиторий для работы с событиями.
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Добавляет событие.
    /// </summary>
    /// <param name="eventDto">Событие.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(EventDto eventDto, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет событие.
    /// </summary>
    /// <param name="eventDto">Событие.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateAsync(EventDto eventDto, CancellationToken cancellationToken);
    
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