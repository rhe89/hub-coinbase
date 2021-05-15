using AutoMapper;
using Coinbase.Core.Integration;
using Coinbase.Data;
using Coinbase.Data.AutoMapper;
using Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Coinbase.HostedServices.ServiceBusQueueHost.Commands;
using Coinbase.HostedServices.ServiceBusQueueHost.QueueListenerServices;
using Coinbase.Integration;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coinbase.HostedServices.ServiceBusQueueHost
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IMessageSender, MessageSender>();
            serviceCollection.AddTransient<IUpdateCoinbaseAccountsCommandHandler, UpdateCoinbaseAccountsCommandHandler>();
            serviceCollection.AddTransient<IUpdateCoinbaseExchangeRatesCommandHandler, UpdateCoinbaseExchangeRatesCommandHandler>();
            serviceCollection.AddTransient<ICoinbaseConnector, CoinbaseConnector>();
            
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddCoinbaseProfiles();
            });
        }

        protected override void AddQueueListenerServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<UpdateCoinbaseAccountsCommand>();
            serviceCollection.AddTransient<UpdateCoinbaseExchangeRatesCommand>();  
            
            serviceCollection.AddHostedService<UpdateCoinbaseExchangeRatesQueueListener>();
            serviceCollection.AddHostedService<UpdateCoinbaseAccountsQueueListener>();
        }
    }
}