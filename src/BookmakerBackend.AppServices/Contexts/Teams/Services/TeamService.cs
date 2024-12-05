using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Teams.Repositories;
using BookmakerBackend.Contracts.Teams;

namespace BookmakerBackend.AppServices.Contexts.Teams.Services;

/// <inheritdoc cref="ITeamService"/>
public class TeamService(ITeamRepository repository, IMapper mapper) : ITeamService
{
    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TeamDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ICollection<TeamDto>> SearchAsync(string searchString, CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(searchString, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(AddTeamRequest request, CancellationToken cancellationToken)
    {
        var team = mapper.Map<TeamDto>(request);
        await repository.AddAsync(team, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Guid id, UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var team = await repository.GetByIdAsync(id, cancellationToken);
        team.Name = request.Name;
        team.Country = request.Country;
        await repository.UpdateAsync(team, cancellationToken);
    }
}