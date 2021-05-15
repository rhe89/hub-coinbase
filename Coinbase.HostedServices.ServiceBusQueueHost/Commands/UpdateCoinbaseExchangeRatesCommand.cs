using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.HostedServices.Commands.Core;
using Hub.HostedServices.ServiceBusQueue.Commands;
using Hub.ServiceBus.Core;

namespace Coinbase.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseExchangeRatesCommand : ServiceBusQueueCommand, ICommandWithConsumers
    {
        private readonly IUpdateCoinbaseExchangeRatesCommandHandler _updateCoinbaseExchangeRatesCommandHandler;
        private readonly IMessageSender _messageSender;

        public UpdateCoinbaseExchangeRatesCommand(IUpdateCoinbaseExchangeRatesCommandHandler updateCoinbaseExchangeRatesCommandHandler,
            IMessageSender messageSender)
        {
            _updateCoinbaseExchangeRatesCommandHandler = updateCoinbaseExchangeRatesCommandHandler;
            _messageSender = messageSender;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            await _updateCoinbaseExchangeRatesCommandHandler.UpdateExchangeRates();
        }

        public async Task NotifyConsumers()
        {
            await _messageSender.AddToQueue(QueueNames.CoinbaseExchangeRatesUpdated);
        }

        public override string QueueName => QueueNames.UpdateCoinbaseExchangeRates;
    }
}