using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Integration;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Hub.Storage.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Coinbase.BackgroundTasks
{
    public class UpdateExchangeRatesTask : BackgroundTask
    {
        private readonly ILogger<UpdateExchangeRatesTask> _logger;
        private readonly ICoinbaseConnector _coinbaseConnector;
        private readonly IHubDbRepository _dbRepository;

        public UpdateExchangeRatesTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ILogger<UpdateExchangeRatesTask> logger,
            ICoinbaseConnector coinbaseConnector,
            IHubDbRepository dbRepository) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _coinbaseConnector = coinbaseConnector;
            _dbRepository = dbRepository;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            var exchangeRatesInDb = await _dbRepository.AllAsync<ExchangeRate, ExchangeRateDto>();
            
            _logger.LogInformation($"Got {exchangeRatesInDb.Count} exchange rates from database");
            
            foreach (var exchangeRateInDb in exchangeRatesInDb)
            {
                _logger.LogInformation($"Updating {exchangeRateInDb.Currency}");
                
                var exchangeRateFromCoinbase =
                    await _coinbaseConnector.GetExchangeRatesForCurrency(exchangeRateInDb.Currency);

                if (exchangeRateFromCoinbase == null)
                {
                    _logger.LogError($"Data from Coinbase for exchange rate {exchangeRateInDb.Currency} was null");
                    continue;
                }
                
                var nokRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.NOK];
                var usdRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.USD];
                var eurRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.EUR];

                exchangeRateInDb.NOKRate = nokRate;
                exchangeRateInDb.USDRate = usdRate;
                exchangeRateInDb.EURRate = eurRate;
            }
            
            _dbRepository.BulkUpdate<ExchangeRate, ExchangeRateDto>(exchangeRatesInDb);
            
            await _dbRepository.SaveChangesAsync();
            
            _logger.LogInformation($"Finished updating {exchangeRatesInDb.Count} exchange rates");
        }
    }
}