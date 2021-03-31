using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.Core.Entities;
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
            await UpdateAccountAssets();
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

                try
                {
                    await UpdateAccount(coinbaseAccounts, dbAccount, assets);
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Failed updating account {dbAccount.Currency}. Continuing", e.Message);
                }
            }

            await _dbRepository.ExecuteQueueAsync();
            
            _logger.LogInformation($"Done updating cryptocurrencies.");
        }

        private async Task UpdateAccount(IList<Models.Account> coinbaseAccounts, AccountDto dbAccount, IList<AssetDto> assets)
        {
            var correspondingCoinbaseAccount =
                coinbaseAccounts.FirstOrDefault(x => x.Currency.Code == dbAccount.Currency);

            if (correspondingCoinbaseAccount == null)
            {
                _logger.LogInformation($"Couldn't get account {dbAccount.Currency} from Coinbase. Skipping.");
                return;
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

                _dbRepository.QueueUpdate<Asset, AssetDto>(existingAsset);
            }
            else
            {
                _logger.LogInformation($"Adding assets for currency {dbAccount.Currency}");

                var asset = new AssetDto
                {
                    AccountId = dbAccount.Id,
                    Value = valueInNok
                };

                _dbRepository.QueueAdd<Asset, AssetDto>(asset);
            }
        }

        private async Task<decimal> GetExchangeRateInNok(string currency)
        {
            var exchangeRates = await _coinbaseConnector.GetExchangeRatesForCurrency(currency);
            
            var hasExchangeRateInNok = exchangeRates.Rates.TryGetValue(ExchangeRateConstants.NOK, out var exchangeRateInNok);

            if (!hasExchangeRateInNok)
            {
                _logger.LogWarning(
                    $"No exchange rates in {ExchangeRateConstants.NOK} for {currency} exists at Coinbase");
            }
            
            return exchangeRateInNok;
            
        }
    }
}