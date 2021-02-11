using AutoMapper;

namespace Coinbase.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddCoinbaseProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<AccountMapperProfile>();
            mapperConfigurationExpression.AddProfile<AssetsMapperProfile>();
            mapperConfigurationExpression.AddProfile<ExchangeRateMapperProfile>();

            return mapperConfigurationExpression;
        }
    }
}