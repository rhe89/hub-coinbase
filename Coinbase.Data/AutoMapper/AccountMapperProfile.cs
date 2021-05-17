using AutoMapper;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;

namespace Coinbase.Data.AutoMapper
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Balance, 
                    opt => opt.MapFrom(x => x.CurrentBalance))
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(x => x.Currency))
                .ReverseMap();
            
        }
    }
}