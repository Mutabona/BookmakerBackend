using AutoMapper;
using BookmakerBackend.Contracts.Bets;
using BookmakerBackend.Domain.Domain;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Registrar.MapProfiles;

/// <summary>
/// Профиль ставок.
/// </summary>
public class BetProfile : Profile
{
    public BetProfile()
    {
        CreateMap<AddBetRequest, BetDto>(MemberList.None);
        
        CreateMap<BetDto, Bet>(MemberList.None)
            .ForMember(x => x.Status, map => map.MapFrom(x => Enum.Parse<BetStatus>(x.Status)));
        
        CreateMap<Bet, BetDto>(MemberList.None)
            .ForMember(x => x.Status, map => map.MapFrom(x => x.Status.ToString()))
            .ForMember(x => x.CoefficientType, map => map.MapFrom(x => x.Coefficient.Type.ToString()))
            .ForMember(x => x.EventTitle, map => map.MapFrom(x => x.Coefficient.Event.Name))
            .ForMember(x => x.Team1Id, map => map.MapFrom(x => x.Coefficient.Event.Team1Id))
            .ForMember(x => x.Team2Id, map => map.MapFrom(x => x.Coefficient.Event.Team2Id));
    }
}