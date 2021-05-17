using System.Threading.Tasks;

namespace Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public interface IUpdateCoinbaseAccountBalanceHistoryCommandHandler
    {
        Task UpdateAccountBalance();
    }
}