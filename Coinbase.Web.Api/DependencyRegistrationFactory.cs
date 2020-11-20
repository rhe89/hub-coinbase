using AutoMapper;
using Coinbase.BackgroundTasks;
using Coinbase.Core.Integration;
using Coinbase.Core.Providers;
using Coinbase.Data;
using Coinbase.Data.AutoMapper;
using Coinbase.Integration;
using Coinbase.Providers;
using Coinbase.Web.Api.Services;
using Hub.Storage.Repository.AutoMapper;
using Hub.Web.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.Web.Api
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactoryWithHostedServiceBase<CoinbaseDbContext>
    {
        public DependencyRegistrationFactory() : base("SQL_DB_COINBASE", "Coinbase.Data")
        {
        }

        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddTransient<IAccountProvider, AccountProvider>();
            serviceCollection.TryAddTransient<IAssetsProvider, AssetsProvider>();
            serviceCollection.TryAddTransient<IAssetsService, AssetsService>();
            serviceCollection.TryAddTransient<IAccountService, AccountService>();
            serviceCollection.TryAddScoped<UpdateAccountsTask>();
            serviceCollection.TryAddScoped<ICoinbaseConnector, CoinbaseConnector>();
            
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddHostedServiceProfiles();
                c.AddCoinbaseProfiles();
            });
        }
    }
}