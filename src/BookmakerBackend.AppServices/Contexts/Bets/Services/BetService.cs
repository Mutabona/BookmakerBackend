using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Bets.Repositories;
using BookmakerBackend.Contracts.Bets;

namespace BookmakerBackend.AppServices.Contexts.Bets.Services;

/// <inheritdoc cref="IBetService"/>
public class BetService(IBetRepository repository, IMapper mapper) : IBetService
{
    /// <inheritdoc/>
    public async Task AddAsync(string username, Guid coefficientId, AddBetRequest request, CancellationToken cancellationToken)
    {
        var bet = mapper.Map<BetDto>(request);
        bet.CoefficientId = coefficientId;
        bet.Username = username;
        await repository.AddAsync(bet, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<BetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ICollection<BetDto>> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await repository.GetByUsernameAsync(username, cancellationToken);
    }
}