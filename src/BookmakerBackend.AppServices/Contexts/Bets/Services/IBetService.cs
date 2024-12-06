using BookmakerBackend.Contracts.Bets;

namespace BookmakerBackend.AppServices.Contexts.Bets.Services;

/// <summary>
/// Сервися для работы со ставками.
/// </summary>
public interface IBetService
{
    /// <summary>
    /// Создаёт ставку по модели запроса.
    /// </summary>
    /// <param name="username">Лоигн пользователя, сделавшего ставку.</param>
    /// <param name="coefficientId">Идентификатор коэффициента.</param>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(string username, Guid coefficientId, AddBetRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет ставку по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
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
}