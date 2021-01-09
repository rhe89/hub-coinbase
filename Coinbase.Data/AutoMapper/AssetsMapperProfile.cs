using AutoMapper;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;

namespace Coinbase.Data.AutoMapper
{
    public class AssetsMapperProfile : Profile
    {
        public AssetsMapperProfile()
        {
            CreateMap<Asset, AssetDto>()
                .ForMember(x => x.Account, y => y.Ignore())
            .ReverseMap();
        }
    }
}