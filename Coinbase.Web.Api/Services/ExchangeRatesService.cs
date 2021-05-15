using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Factories;
using Coinbase.Core.Integration;
using Coinbase.Core.Providers;
using Microsoft.Extensions.Logging;

namespace Coinbase.Web.Api.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IExchangeRateFactory _exchangeRateFactory;
        private readonly ICoinbaseConnector _coinbaseConnector;
        private readonly ILogger<ExchangeRatesService> _logger;

        public ExchangeRatesService(IExchangeRateProvider exchangeRateProvider,
            IExchangeRateFactory exchangeRateFactory,
            ICoinbaseConnector coinbaseConnector,
            ILogger<ExchangeRatesService> logger)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _exchangeRateFactory = exchangeRateFactory;
            _coinbaseConnector = coinbaseConnector;
            _logger = logger;
        }
        
        public async Task<IList<ExchangeRateDto>> GetExchangeRates()
        {
            var exchangeRateDtos = await _exchangeRateProvider.GetExchangeRates();

            return exchangeRateDtos;
        }

        public async Task<ExchangeRateDto> GetExchangeRateForCurrency(string currency)
        {
            var exchangeRateDto = await _exchangeRateProvider.GetExchangeRate(currency) ?? await GetExchangeRateFromCoinbaseAndAddToDb(currency);

            return exchangeRateDto;
        }


        private async Task<ExchangeRateDto> GetExchangeRateFromCoinbaseAndAddToDb(string currency)
        {
            var exchangeRateFromCoinbase =
                await _coinbaseConnector.GetExchangeRatesForCurrency(currency);

            if (exchangeRateFromCoinbase == null)
            {
                _logger.LogError($"Data from Coinbase for exchange rate {currency} was null");
                return null;
            }
                
            var nokRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.NOK];
            var usdRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.USD];
            var eurRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.EUR];

            return _exchangeRateFactory.Add(currency, nokRate, usdRate, eurRate);
        }
    }
}