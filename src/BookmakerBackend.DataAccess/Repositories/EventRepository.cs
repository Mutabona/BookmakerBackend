using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Events.Repositories;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="IEventRepository"/>
public class EventRepository : IEventRepository
{
    private DbContext DbContext { get; }
    private DbSet<Event> Events { get; }
    private readonly IMapper _mapper;

    public EventRepository(DbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        _mapper = mapper;
        Events = DbContext.Set<Event>();
    }
}