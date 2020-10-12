using Coinbase.BackgroundTasks;
using Coinbase.Data;
using Coinbase.Integration;
using Hub.HostedServices.Tasks;
using Hub.HostedServices.Timer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.BackgroundWorker
{
    public class CoinbaseWorkerTimerHostBuilder : TimerHostBuilder<CoinbaseDbContext>
    {
        internal CoinbaseWorkerTimerHostBuilder(string[] args) : base(args, "SQL_DB_COINBASE")
        {
        }

        protected override void RegisterDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IBackgroundTask, UpdateAccountsTask>();
            serviceCollection.TryAddSingleton<ICoinbaseConnector, CoinbaseConnector>();
        }
    }
}