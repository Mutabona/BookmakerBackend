using AutoMapper;
using BookmakerBackend.Contracts.Users;
using BookmakerBackend.Domain.Domain;

namespace BookmakerBackend.Registrar.MapProfiles;

/// <summary>
/// Профиль пользователя.
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, UserDto>(MemberList.None);
        
        CreateMap<UserDto, User>(MemberList.None).ReverseMap();
    }
}