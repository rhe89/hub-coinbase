using Coinbase.HostedServices.ServiceBusQueueHost.Commands;
using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Logging;

namespace Coinbase.HostedServices.ServiceBusQueueHost.QueueListenerServices
{
    public class UpdateCoinbaseExchangeRatesQueueListener : ServiceBusHostedService
    {
        public UpdateCoinbaseExchangeRatesQueueListener(ILogger<UpdateCoinbaseExchangeRatesQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            UpdateCoinbaseExchangeRatesCommand command, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 command, 
                                                 queueProcessor)
        {
        }
    }
}