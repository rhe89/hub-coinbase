using System;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Integration;
using Hub.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public class UpdateCoinbaseExchangeRatesCommandHandler : IUpdateCoinbaseExchangeRatesCommandHandler
    {
        private readonly ILogger<UpdateCoinbaseExchangeRatesCommandHandler> _logger;
        private readonly ICoinbaseConnector _coinbaseConnector;
        private readonly IHubDbRepository _dbRepository;

        public UpdateCoinbaseExchangeRatesCommandHandler(ILogger<UpdateCoinbaseExchangeRatesCommandHandler> logger,
            ICoinbaseConnector coinbaseConnector,
            IHubDbRepository dbRepository)
        {
            _logger = logger;
            _coinbaseConnector = coinbaseConnector;
            _dbRepository = dbRepository;
        }

        public async Task UpdateExchangeRates()
        {
            var exchangeRatesInDb = await _dbRepository.AllAsync<ExchangeRate, ExchangeRateDto>();
            
            _logger.LogInformation($"Got {exchangeRatesInDb.Count} exchange rates from database");
            
            var exchangeRatesCount = exchangeRatesInDb.Count;

            var counter = 1;
            
            foreach (var exchangeRateInDb in exchangeRatesInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {exchangeRatesCount}: {exchangeRateInDb.Currency}.");

                try
                {
                    await UpdateExchangeRate(exchangeRateInDb);
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Failed updating exchange rate {exchangeRateInDb.Currency}. Continuing", e.Message);
                }
            }
            
            await _dbRepository.ExecuteQueueAsync();
            
            _logger.LogInformation($"Finished updating {exchangeRatesInDb.Count} exchange rates");
        }

        private async Task UpdateExchangeRate(ExchangeRateDto exchangeRateInDb)
        {
            var exchangeRateFromCoinbase =
                await _coinbaseConnector.GetExchangeRatesForCurrency(exchangeRateInDb.Currency);

            if (exchangeRateFromCoinbase == null)
            {
                _logger.LogError($"Data from Coinbase for exchange rate {exchangeRateInDb.Currency} was null");
                return;
            }

            var nokRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.NOK];
            var usdRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.USD];
            var eurRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.EUR];

            exchangeRateInDb.NOKRate = nokRate;
            exchangeRateInDb.USDRate = usdRate;
            exchangeRateInDb.EURRate = eurRate;

            _dbRepository.QueueUpdate<ExchangeRate, ExchangeRateDto>(exchangeRateInDb);
        }
    }
}