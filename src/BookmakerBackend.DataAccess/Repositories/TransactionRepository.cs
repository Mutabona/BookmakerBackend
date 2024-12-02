using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Transactions.Repositories;
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
}