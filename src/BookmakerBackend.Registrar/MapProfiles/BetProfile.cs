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
            .ForMember(x => x.Status, map => map.MapFrom(x => x.Status.ToString()));
    }
}