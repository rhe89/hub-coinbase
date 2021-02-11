using Coinbase.Core.Dto.Data;

namespace Coinbase.Core.Factories
{
    public interface IExchangeRateFactory
    {
        ExchangeRateDto Add(string currency, decimal nokRate, decimal usdRate, decimal eurRate);
    }
}