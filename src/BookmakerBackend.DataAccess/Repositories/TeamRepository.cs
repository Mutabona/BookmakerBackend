using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookmakerBackend.AppServices.Contexts.Teams.Repositories;
using BookmakerBackend.AppServices.Exceptions;
using BookmakerBackend.Contracts.Teams;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="ITeamRepository"/>
public class TeamRepository : ITeamRepository
{
    private DbContext DbContext { get; }
    private DbSet<Team> Teams { get; }
    private readonly IMapper _mapper;

    public TeamRepository(DbContext context, IMapper mapper)
    {
        DbContext = context;
        _mapper = mapper;
        Teams = context.Set<Team>();
    }

    /// <inheritdoc/>
    public async Task AddAsync(TeamDto team, CancellationToken cancellationToken)
    {
        var teamEntity = _mapper.Map<Team>(team);
        await Teams.AddAsync(teamEntity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var teamEntity = await Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (teamEntity == null) throw new EntityNotFoundException();
        Teams.Remove(teamEntity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TeamDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var teamEntity = await Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (teamEntity == null) throw new EntityNotFoundException();
        return _mapper.Map<TeamDto>(teamEntity);
    }

    /// <inheritdoc/>
    public async Task<ICollection<TeamDto>> SearchAsync(string searchString, CancellationToken cancellationToken)
    {
        var teams = await Teams.AsNoTracking()
            .Where(x => x.Name.ToLower().Contains(searchString.ToLower()))
            .ProjectTo<TeamDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return teams;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TeamDto team, CancellationToken cancellationToken)
    {
        var teamEntity = _mapper.Map<Team>(team);
        
        Teams.Update(teamEntity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}