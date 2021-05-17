using AutoMapper;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;

namespace Coinbase.Data.AutoMapper
{
    public class AccountBalanceMapperProfile : Profile
    {
        public AccountBalanceMapperProfile()
        {
            CreateMap<AccountBalance, AccountBalanceDto>()
                .ForMember(dest => dest.Balance, 
                    opt => opt.MapFrom(x => x.Value))
                .ReverseMap();
        }
    }
}