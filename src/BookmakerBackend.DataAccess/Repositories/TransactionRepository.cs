using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookmakerBackend.AppServices.Contexts.Transactions.Repositories;
using BookmakerBackend.Contracts.Transactions;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="ITransactionRepository"/>
public class TransactionRepository : ITransactionRepository
{
    private DbContext DbContext { get; }
    private DbSet<Transaction> Transactions { get; }
    private readonly IMapper _mapper;

    public TransactionRepository(DbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Transactions = dbContext.Set<Transaction>();
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ICollection<TransactionDto>> GetTransactionsByUserLoginAsync(string login, CancellationToken cancellationToken)
    {
        var transactions = await Transactions
            .AsNoTracking()
            .Where(x => x.Username == login)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return transactions;
    }

    /// <inheritdoc/>
    public async Task AddAsync(TransactionDto transaction, CancellationToken cancellationToken)
    {
        var transactionEntity = _mapper.Map<Transaction>(transaction);
        await Transactions.AddAsync(transactionEntity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}