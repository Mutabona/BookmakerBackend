using AutoMapper;
using BookmakerBackend.Contracts.Transactions;
using BookmakerBackend.Domain.Domain;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Registrar.MapProfiles;

/// <summary>
/// Профиль транзакции.
/// </summary>
public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<AddTransactionRequest, TransactionDto>(MemberList.None);
        
        CreateMap<Transaction, TransactionDto>(MemberList.None)
            .ForMember(x => x.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        
        CreateMap<TransactionDto, Transaction>(MemberList.None)
            .ForMember(x => x.Type, opt => opt.MapFrom(src => (TransactionType)Enum.Parse(typeof(TransactionType), src.Type)));
    }
}