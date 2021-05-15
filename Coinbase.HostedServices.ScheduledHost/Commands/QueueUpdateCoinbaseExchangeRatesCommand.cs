using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Hub.HostedServices.Commands.Configuration.Core;
using Hub.HostedServices.Schedule.Commands;
using Hub.ServiceBus.Core;

namespace Coinbase.HostedServices.ScheduledHost.Commands
{
    public class QueueUpdateCoinbaseExchangeRatesCommand : ScheduledCommand
    {
        private readonly IMessageSender _messageSender;

        public QueueUpdateCoinbaseExchangeRatesCommand(ICommandConfigurationProvider commandConfigurationProvider,
            ICommandConfigurationFactory commandConfigurationFactory,
            IMessageSender messageSender) : base(commandConfigurationProvider, commandConfigurationFactory)
        {
            _messageSender = messageSender;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseExchangeRates);
        }

        
    }
}