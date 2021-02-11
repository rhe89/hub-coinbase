using AutoMapper;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;

namespace Coinbase.Data.AutoMapper
{
    public class ExchangeRateMapperProfile : Profile
    {
        public ExchangeRateMapperProfile()
        {
            CreateMap<ExchangeRate, ExchangeRateDto>()
                .ReverseMap();
        }
    }
}