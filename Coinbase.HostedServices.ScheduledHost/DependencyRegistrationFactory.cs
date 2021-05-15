using Coinbase.Data;
using Coinbase.HostedServices.ScheduledHost.Commands;
using Hub.HostedServices.Schedule;
using Hub.HostedServices.Schedule.Commands;
using Hub.ServiceBus;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coinbase.HostedServices.ScheduledHost
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IMessageSender, MessageSender>();
            serviceCollection.AddSingleton<IScheduledCommand, QueueUpdateCoinbaseAccountsCommand>();
            serviceCollection.AddSingleton<IScheduledCommand, QueueUpdateCoinbaseExchangeRatesCommand>();
        }
    }
}