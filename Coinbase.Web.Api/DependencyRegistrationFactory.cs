using AutoMapper;
using Coinbase.Core.Factories;
using Coinbase.Core.Integration;
using Coinbase.Core.Providers;
using Coinbase.Data;
using Coinbase.Data.AutoMapper;
using Coinbase.Factories;
using Coinbase.Integration;
using Coinbase.Providers;
using Hub.Web.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.Web.Api
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseDbContext>
    {
        public DependencyRegistrationFactory() : base("SQL_DB_COINBASE", "Coinbase.Data")
        {
        }

        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddTransient<IAccountProvider, AccountProvider>();
            serviceCollection.TryAddTransient<IAccountBalanceProvider, AccountBalanceProvider>();
            serviceCollection.TryAddTransient<IExchangeRateProvider, ExchangeRateProvider>();
            serviceCollection.TryAddTransient<IExchangeRateFactory, ExchangeRateFactory>();
            serviceCollection.TryAddTransient<ICoinbaseConnector, CoinbaseConnector>();
            
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddCoinbaseProfiles();
            });
        }
    }
}