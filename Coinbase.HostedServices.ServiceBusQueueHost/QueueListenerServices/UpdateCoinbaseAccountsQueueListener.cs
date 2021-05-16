using Coinbase.HostedServices.ServiceBusQueueHost.Commands;
using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Coinbase.HostedServices.ServiceBusQueueHost.QueueListenerServices
{
    public class UpdateCoinbaseAccountsQueueListener : ServiceBusHostedService
    {
        public UpdateCoinbaseAccountsQueueListener(ILogger<UpdateCoinbaseAccountsQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            IConfiguration configuration,
            UpdateCoinbaseAccountsCommand command, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 configuration,
                                                 command, 
                                                 queueProcessor)
        {
        }
    }
}