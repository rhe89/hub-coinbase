using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Entities;
using Coinbase.Core.Exceptions;
using Coinbase.Core.Integration;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Hub.Storage.Core.Repository;
using Microsoft.Extensions.Logging;
using Account = Coinbase.Core.Entities.Account;
using AccountDto = Coinbase.Core.Dto.Data.AccountDto;
using AssetDto = Coinbase.Core.Dto.Data.AssetDto;

namespace Coinbase.BackgroundTasks
{
    public class UpdateAccountsTask : BackgroundTask
    {
        private readonly ILogger<UpdateAccountsTask> _logger;
        private readonly ICoinbaseConnector _coinbaseConnector;
        private readonly IHubDbRepository _dbRepository;

        public UpdateAccountsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ILogger<UpdateAccountsTask> logger,
            ICoinbaseConnector coinbaseConnector,
            IHubDbRepository dbRepository) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _coinbaseConnector = coinbaseConnector;
            _dbRepository = dbRepository;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            _dbRepository.ToggleDispose(false);

            try
            {
                await UpdateAccountAssets();

                _dbRepository.ToggleDispose(true);

                await _dbRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                _dbRepository.ToggleDispose(true);

                throw;
            }
        }

        private async Task UpdateAccountAssets()
        {
            var accountsInDb = await _dbRepository.AllAsync<Account, AccountDto>();

            var assets = await _dbRepository.AllAsync<Asset, AssetDto>();

            var coinbaseAccounts = await  _coinbaseConnector.GetAccounts();

            var accountsCount = accountsInDb.Count;

            var counter = 1;

            foreach (var dbAccount in accountsInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {accountsCount}: {dbAccount.Currency}.");

                var correspondingCoinbaseAccount =
                    coinbaseAccounts.FirstOrDefault(x => x.Currency.Code == dbAccount.Currency);

                if (correspondingCoinbaseAccount == null)
                {
                    _logger.LogInformation($"Couldn't get account {dbAccount.Currency} from Coinbase. Skipping.");
                    continue;
                }

                if (correspondingCoinbaseAccount.Balance.Amount == 0)
                {
                    _logger.LogInformation($"Assets is 0. Skipping account {dbAccount.Currency}.");
                    continue;
                }

                var existingAsset = assets.FirstOrDefault(x =>
                    x.CreatedDate.Date == DateTime.Now.Date &&
                    x.AccountId == dbAccount.Id);

                var exchangeRateInNok = await GetExchangeRateInNok(dbAccount.Currency);

                var valueInNok = (int) (correspondingCoinbaseAccount.Balance.Amount * exchangeRateInNok);

                if (existingAsset != null)
                {
                    _logger.LogInformation($"Updating assets for {dbAccount.Currency}");

                    existingAsset.Value = valueInNok;

                    _dbRepository.Update<Asset, AssetDto>(existingAsset);
                }
                else
                {
                    _logger.LogInformation($"Adding assets for currency {dbAccount.Currency}");

                    var asset = new AssetDto
                    {
                        AccountId = dbAccount.Id,
                        Value = valueInNok
                    };

                    _dbRepository.Add<Asset, AssetDto>(asset);
                }
            }

            _logger.LogInformation($"Done updating cryptocurrencies.");
        }
        
        private async Task<decimal> GetExchangeRateInNok(string currency)
        {
            var exchangeRates = await _coinbaseConnector.GetExchangeRatesForCurrency(currency);
            
            var hasExchangeRateInNok = exchangeRates.Rates.TryGetValue("NOK", out var exchangeRateInNok);

            if (!hasExchangeRateInNok)
            {
                throw new CoinbaseConnectorException(
                    $"Error occured when getting exchange rates for {currency} from Coinbase. No exchange rate in NOK exists.");
            }
            
            return exchangeRateInNok;
            
        }
    }
}