using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookmakerBackend.AppServices.Contexts.Events.Repositories;
using BookmakerBackend.AppServices.Exceptions;
using BookmakerBackend.Contracts.Events;
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

    /// <inheritdoc/>
    public async Task AddAsync(EventDto eventDto, CancellationToken cancellationToken)
    {
        var eventEntity = _mapper.Map<Event>(eventDto);
        await Events.AddAsync(eventEntity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(EventDto eventDto, CancellationToken cancellationToken)
    {
        var eventEntity = _mapper.Map<Event>(eventDto);
        Events.Update(eventEntity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ICollection<EventDto>> SearchAsync(string searchString, CancellationToken cancellationToken)
    {
        var events = await Events
            .AsNoTracking()
            .Where(x => x.Name.ToLower().Contains(searchString.ToLower()))
            .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return events;
    }

    /// <inheritdoc/>
    public async Task<EventDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var eventEntity = await Events.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
        
        if (eventEntity == null) throw new EntityNotFoundException();
        
        return _mapper.Map<EventDto>(eventEntity);
    }
}