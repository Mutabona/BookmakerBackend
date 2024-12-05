using BookmakerBackend.Contracts.Teams;

namespace BookmakerBackend.AppServices.Contexts.Teams.Repositories;

/// <summary>
/// Репозиторий для работы с командами.
/// </summary>
public interface ITeamRepository
{
    /// <summary>
    /// Добавляет команду.
    /// </summary>
    /// <param name="team">Команда.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(TeamDto team, CancellationToken cancellationToken);
    
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
    /// Обновляет команду.
    /// </summary>
    /// <param name="team">Команда.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateAsync(TeamDto team, CancellationToken cancellationToken);
}