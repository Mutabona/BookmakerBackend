using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Transactions.Repositories;
using BookmakerBackend.Contracts.Transactions;

namespace BookmakerBackend.AppServices.Contexts.Transactions.Services;

/// <inheritdoc cref="ITransactionService"/>
public class TransactionService(ITransactionRepository repository, IMapper mapper) : ITransactionService
{
    /// <inheritdoc/>
    public async Task AddAsync(string login, AddTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = mapper.Map<TransactionDto>(request);
        transaction.Username = login;
        await repository.AddAsync(transaction, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ICollection<TransactionDto>> GetTransactionsByUserLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await repository.GetTransactionsByUserLoginAsync(login, cancellationToken);
    }
}