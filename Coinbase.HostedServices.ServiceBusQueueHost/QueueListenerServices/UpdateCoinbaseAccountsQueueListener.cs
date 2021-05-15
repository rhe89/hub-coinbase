using Coinbase.HostedServices.ServiceBusQueueHost.Commands;
using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Logging;

namespace Coinbase.HostedServices.ServiceBusQueueHost.QueueListenerServices
{
    public class UpdateCoinbaseAccountsQueueListener : ServiceBusHostedService
    {
        public UpdateCoinbaseAccountsQueueListener(ILogger<UpdateCoinbaseAccountsQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            UpdateCoinbaseAccountsCommand command, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 command, 
                                                 queueProcessor)
        {
        }
    }
}