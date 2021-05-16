using Coinbase.HostedServices.ServiceBusQueueHost.Commands;
using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Coinbase.HostedServices.ServiceBusQueueHost.QueueListenerServices
{
    public class UpdateCoinbaseExchangeRatesQueueListener : ServiceBusHostedService
    {
        public UpdateCoinbaseExchangeRatesQueueListener(ILogger<UpdateCoinbaseExchangeRatesQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            IConfiguration configuration,
            UpdateCoinbaseExchangeRatesCommand command, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 configuration,
                                                 command, 
                                                 queueProcessor)
        {
        }
    }
}