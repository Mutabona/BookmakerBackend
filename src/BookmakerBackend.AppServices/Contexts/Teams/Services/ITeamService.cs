using BookmakerBackend.Contracts.Teams;

namespace BookmakerBackend.AppServices.Contexts.Teams.Services;

/// <summary>
/// Сервися для работы с командами.
/// </summary>
public interface ITeamService
{
    /// <summary>
    /// Удаляет команду по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает команду по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель команды</returns>
    Task<TeamDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Выполняет поиск команды по строке поиска.
    /// </summary>
    /// <param name="searchString">Стока поиска.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей команд.</returns>
    Task<ICollection<TeamDto>> SearchAsync(string searchString, CancellationToken cancellationToken);
    
    /// <summary>
    /// Добавляет команду по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(AddTeamRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет команду по модели запроса.
    /// </summary>
    /// <param name="id">Идентификатор комманды.</param>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateAsync(Guid id, UpdateTeamRequest request, CancellationToken cancellationToken);
}