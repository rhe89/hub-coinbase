using System.Threading.Tasks;

namespace Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public interface IUpdateCoinbaseExchangeRatesCommandHandler
    {
        Task UpdateExchangeRates();
    }
}