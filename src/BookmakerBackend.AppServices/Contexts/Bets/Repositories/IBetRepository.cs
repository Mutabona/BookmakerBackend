using BookmakerBackend.Contracts.Bets;

namespace BookmakerBackend.AppServices.Contexts.Bets.Repositories;

/// <summary>
/// Репозиторий для работы со ставками.
/// </summary>
public interface IBetRepository
{
    /// <summary>
    /// Добавляет ставку.
    /// </summary>
    /// <param name="bet">Ставка.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(BetDto bet, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает ставку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель ставки.</returns>
    Task<BetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает ставки по логину пользователя.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей ставок.</returns>
    Task<ICollection<BetDto>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет ставку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}