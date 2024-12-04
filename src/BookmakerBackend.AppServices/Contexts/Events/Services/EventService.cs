using BookmakerBackend.AppServices.Contexts.Events.Repositories;

namespace BookmakerBackend.AppServices.Contexts.Events.Services;

/// <inheritdoc cref="IEventService"/>
public class EventService(IEventRepository repository) : IEventService
{
    
}