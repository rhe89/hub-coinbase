using System.Threading.Tasks;
using Coinbase.Core.Dto.Api;
using Coinbase.Core.Integration;

namespace Coinbase.Web.Api.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly ICoinbaseConnector _coinbaseConnector;

        public ExchangeRatesService(ICoinbaseConnector coinbaseConnector)
        {
            _coinbaseConnector = coinbaseConnector;
        }

        public async Task<ExchangeRatesDto> GetExchangeRateForCurrency(string currency)
        {
            var exchangeRate = await _coinbaseConnector.GetExchangeRatesForCurrency(currency);

            return new ExchangeRatesDto
            {
                Currency = exchangeRate.Currency,
                Rates = exchangeRate.Rates
            };
        }
    }
}