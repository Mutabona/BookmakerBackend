using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookmakerBackend.AppServices.Contexts.Bets.Repositories;
using BookmakerBackend.AppServices.Exceptions;
using BookmakerBackend.Contracts.Bets;
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

    /// <inheritdoc/>
    public async Task AddAsync(BetDto bet, CancellationToken cancellationToken)
    {
        var betEntity = _mapper.Map<Bet>(bet);
        await Bets.AddAsync(betEntity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<BetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bet = await Bets
            .AsNoTracking()
            .Include(b => b.Coefficient)
            .Include(b => b.Coefficient.Event)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken: cancellationToken);
        
        if (bet == null) throw new EntityNotFoundException();
        
        return _mapper.Map<BetDto>(bet);
    }

    /// <inheritdoc/>
    public async Task<ICollection<BetDto>> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var bets = await Bets
            .AsNoTracking()
            .Where(b => b.Username == username)
            .Include(b => b.Coefficient)
            .Include(b => b.Coefficient.Event)
            .OrderByDescending(x => x.Coefficient.Event.DateTime)
            .ProjectTo<BetDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return bets;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var bet = await Bets.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (bet == null) throw new EntityNotFoundException();
        Bets.Remove(bet);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}