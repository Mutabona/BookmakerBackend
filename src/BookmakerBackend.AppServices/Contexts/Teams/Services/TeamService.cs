using BookmakerBackend.AppServices.Contexts.Teams.Repositories;

namespace BookmakerBackend.AppServices.Contexts.Teams.Services;

/// <inheritdoc cref="ITeamService"/>
public class TeamService(ITeamRepository repository) : ITeamService
{
    
}