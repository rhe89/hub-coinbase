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
                .ReverseMap();
        }
    }
}