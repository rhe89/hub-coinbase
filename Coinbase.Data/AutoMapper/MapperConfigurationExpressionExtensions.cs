using AutoMapper;

namespace Coinbase.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddCoinbaseProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<AccountMapperProfile>();
            mapperConfigurationExpression.AddProfile<AccountBalanceMapperProfile>();
            mapperConfigurationExpression.AddProfile<ExchangeRateMapperProfile>();

            return mapperConfigurationExpression;
        }
    }
}