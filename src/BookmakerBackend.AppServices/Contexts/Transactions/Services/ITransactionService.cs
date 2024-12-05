using BookmakerBackend.Contracts.Transactions;

namespace BookmakerBackend.AppServices.Contexts.Transactions.Services;

/// <summary>
/// Сервис для работы с транзакциями.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Создаёт транзакцию по модели запроса.
    /// </summary>
    /// <param name="login">Логин пользователя, который создал транзакцию.</param>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(string login, AddTransactionRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает транзакции по логину пользователя.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция транзакций.</returns>
    Task<ICollection<TransactionDto>> GetTransactionsByUserLoginAsync(string login, CancellationToken cancellationToken);
}