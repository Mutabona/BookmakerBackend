using AutoMapper;
using BookmakerBackend.Contracts.Teams;
using BookmakerBackend.Domain.Domain;

namespace BookmakerBackend.Registrar.MapProfiles;

/// <summary>
/// Профиль команды.
/// </summary>
public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<AddTeamRequest, TeamDto>(MemberList.None);
        
        CreateMap<TeamDto, Team>(MemberList.None).ReverseMap();
    }
}