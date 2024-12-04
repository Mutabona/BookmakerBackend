using BookmakerBackend.AppServices.Contexts.Bets.Repositories;

namespace BookmakerBackend.AppServices.Contexts.Bets.Services;

/// <inheritdoc cref="IBetService"/>
public class BetService(IBetRepository repository) : IBetService
{
    
}