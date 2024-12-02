using AutoMapper;
using BookmakerBackend.Contracts.Coefficients;
using BookmakerBackend.Domain.Domain;

namespace BookmakerBackend.Registrar.MapProfiles;

public class CoefficientProfile : Profile
{
    public CoefficientProfile()
    {
        CreateMap<Coefficient, CoefficientDto>(MemberList.None)
            .ForMember(dest => dest.Type, map => map.MapFrom(src => src.Type.ToString()));
    }
}