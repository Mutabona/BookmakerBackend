using BookmakerBackend.Contracts.Transactions;

namespace BookmakerBackend.AppServices.Contexts.Transactions.Repositories;

/// <summary>
/// Репозиторий для работы с транзакциями.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Получает транзакции по логину пользователя.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция транзакций.</returns>
    Task<ICollection<TransactionDto>> GetTransactionsByUserLoginAsync(string login, CancellationToken cancellationToken);
    
    /// <summary>
    /// Добавляет транзакцию.
    /// </summary>
    /// <param name="transaction">Транзакция.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task AddAsync(TransactionDto transaction, CancellationToken cancellationToken);
}