using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Events.Repositories;
using BookmakerBackend.Contracts.Events;

namespace BookmakerBackend.AppServices.Contexts.Events.Services;

/// <inheritdoc cref="IEventService"/>
public class EventService(IEventRepository repository, IMapper mapper) : IEventService
{
    /// <inheritdoc/>
    public async Task AddAsync(AddEventRequest request, CancellationToken cancellationToken)
    {
        var eventEntity = mapper.Map<EventDto>(request);
        eventEntity.Result = "in_progress"; 
        await repository.AddAsync(eventEntity, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Guid id, UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var eventEntity = await repository.GetByIdAsync(id, cancellationToken);
        eventEntity.Name = request.Name;
        eventEntity.DateTime = request.DateTime;
        eventEntity.Sport = request.Sport;
        eventEntity.Result = request.Result;
        await repository.UpdateAsync(eventEntity, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ICollection<EventDto>> SearchAsync(string searchString, CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(searchString, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<EventDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(id, cancellationToken);
    }
}