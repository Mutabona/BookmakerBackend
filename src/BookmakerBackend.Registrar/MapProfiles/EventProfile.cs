using AutoMapper;
using BookmakerBackend.Contracts.Events;
using BookmakerBackend.Domain.Domain;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Registrar.MapProfiles;

/// <summary>
/// Профиль события.
/// </summary>
public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<AddEventRequest, EventDto>(MemberList.None);
        
        CreateMap<EventDto, Event>(MemberList.None)
            .ForMember(x => x.Result, map => map.MapFrom(x => Enum.Parse<ResultStatus>(x.Result)));
        
        CreateMap<Event, EventDto>(MemberList.None)
            .ForMember(x => x.Result, map => map.MapFrom(x => x.Result.ToString()));
    }
}