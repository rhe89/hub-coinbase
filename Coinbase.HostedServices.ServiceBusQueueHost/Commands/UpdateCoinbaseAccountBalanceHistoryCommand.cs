using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.HostedServices.Commands.Core;
using Hub.HostedServices.ServiceBusQueue.Commands;
using Hub.ServiceBus.Core;

namespace Coinbase.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseAccountBalanceHistoryCommand : ServiceBusQueueCommand, ICommandWithConsumers
    {
        private readonly IUpdateCoinbaseAccountBalanceHistoryCommandHandler _updateCoinbaseAccountBalanceHistoryCommandHandler;
        private readonly IMessageSender _messageSender;

        public UpdateCoinbaseAccountBalanceHistoryCommand(IUpdateCoinbaseAccountBalanceHistoryCommandHandler updateCoinbaseAccountBalanceHistoryCommandHandler,
            IMessageSender messageSender)
        {
            _updateCoinbaseAccountBalanceHistoryCommandHandler = updateCoinbaseAccountBalanceHistoryCommandHandler;
            _messageSender = messageSender;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            await _updateCoinbaseAccountBalanceHistoryCommandHandler.UpdateAccountBalance();
        }

        public async Task NotifyConsumers()
        {
            await _messageSender.AddToQueue(QueueNames.CoinbaseAccountBalanceHistoryUpdated);
        }

        public override string QueueName => QueueNames.UpdateCoinbaseAccountBalances;
    }
}