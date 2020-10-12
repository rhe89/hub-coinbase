using Coinbase.BackgroundTasks;
using Coinbase.Data;
using Coinbase.Integration;
using Coinbase.Providers;
using Hub.Web.DependencyRegistration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.Web.Api
{
    public class DependencyRegistrationFactory : ApiWithQueueHostedServiceDependencyRegistrationFactory<CoinbaseDbContext>
    {
        public DependencyRegistrationFactory() : base("SQL_DB_COINBASE", "Coinbase.Data")
        {
        }

        protected override void RegisterDomainDependenciesForApi(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddTransient<IAssetsProvider, AssetsProvider>();
            serviceCollection.TryAddTransient<IAccountProvider, AccountProvider>();
        }

        protected override void RegisterDomainDependenciesForQueueHostedService(IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
        }

        protected override void RegisterSharedDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddScoped<UpdateAccountsTask>();
            serviceCollection.TryAddScoped<ICoinbaseConnector, CoinbaseConnector>();
        }
    }
}