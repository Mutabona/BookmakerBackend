using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Teams.Repositories;
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
}