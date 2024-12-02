using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Bets.Repositories;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="IBetRepository"/>
public class BetRepository : IBetRepository
{
    private DbContext DbContext { get; }
    private DbSet<Bet> Bets { get; }
    private readonly IMapper _mapper;

    public BetRepository(DbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        _mapper = mapper;
        Bets = DbContext.Set<Bet>();
    }
}