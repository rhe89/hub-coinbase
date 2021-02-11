using AutoMapper;
using Coinbase.BackgroundTasks;
using Coinbase.Core.Integration;
using Coinbase.Data;
using Coinbase.Data.AutoMapper;
using Coinbase.Integration;
using Hub.HostedServices.Tasks;
using Hub.HostedServices.TimerHost;
using Hub.Storage.Repository.AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.BackgroundWorker
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactoryBase<CoinbaseDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IBackgroundTask, UpdateAccountsTask>();
            serviceCollection.AddSingleton<IBackgroundTask, UpdateExchangeRatesTask>();
            serviceCollection.TryAddSingleton<ICoinbaseConnector, CoinbaseConnector>();
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddHostedServiceProfiles();
                c.AddCoinbaseProfiles();
            });
        }
    }
}