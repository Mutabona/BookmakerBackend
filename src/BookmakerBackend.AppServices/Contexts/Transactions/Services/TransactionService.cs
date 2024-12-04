using BookmakerBackend.AppServices.Contexts.Transactions.Repositories;

namespace BookmakerBackend.AppServices.Contexts.Transactions.Services;

/// <inheritdoc cref="ITransactionService"/>
public class TransactionService(ITransactionRepository repository) : ITransactionService
{
    
}