using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.HostedServices.Commands.Core;
using Hub.HostedServices.ServiceBusQueue.Commands;
using Hub.ServiceBus.Core;

namespace Coinbase.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseAccountsCommand : ServiceBusQueueCommand, ICommandWithConsumers
    {
        private readonly IUpdateCoinbaseAccountsCommandHandler _updateCoinbaseAccountsCommandHandler;
        private readonly IMessageSender _messageSender;

        public UpdateCoinbaseAccountsCommand(IUpdateCoinbaseAccountsCommandHandler updateCoinbaseAccountsCommandHandler,
            IMessageSender messageSender)
        {
            _updateCoinbaseAccountsCommandHandler = updateCoinbaseAccountsCommandHandler;
            _messageSender = messageSender;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            await _updateCoinbaseAccountsCommandHandler.UpdateAccounts();
        }

        public async Task NotifyConsumers()
        {
            await _messageSender.AddToQueue(QueueNames.CoinbaseAccountsUpdated);
        }

        public override string QueueName => QueueNames.UpdateCoinbaseAccounts;
    }
}