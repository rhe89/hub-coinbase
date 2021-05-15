using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Integration;
using Hub.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Coinbase.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public class UpdateCoinbaseAccountsCommandHandler : IUpdateCoinbaseAccountsCommandHandler
    {
        private readonly ILogger<UpdateCoinbaseAccountsCommandHandler> _logger;
        private readonly ICoinbaseConnector _coinbaseApiConnector;
        private readonly IHubDbRepository _dbRepository;

        public UpdateCoinbaseAccountsCommandHandler(ILogger<UpdateCoinbaseAccountsCommandHandler> logger,
            ICoinbaseConnector coinbaseApiConnector,
            IHubDbRepository dbRepository)
        {
            _logger = logger;
            _coinbaseApiConnector = coinbaseApiConnector;
            _dbRepository = dbRepository;
        }
        
        public async Task UpdateAccountAssets()
        {
            var accountsInDb = await _dbRepository.AllAsync<Account, AccountDto>();
            
            var coinbaseProAccounts = await _coinbaseApiConnector.GetAccounts();
            
            var accountsCount = accountsInDb.Count;

            var counter = 1;

            foreach (var dbAccount in accountsInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {accountsCount}: {dbAccount.Currency}.");

                try
                {
                    await UpdateAccount(dbAccount, coinbaseProAccounts);
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Failed updating account {dbAccount.Currency}. Continuing", e.Message);
                }
            }

            await _dbRepository.ExecuteQueueAsync();

            _logger.LogInformation("Done updating cryptocurrencies");
        }

        private async Task UpdateAccount(AccountDto dbAccount, IEnumerable<Models.Account> coinbaseAccounts)
        {

            var correspondingCoinbaseAccount =
                coinbaseAccounts.FirstOrDefault(x => x.Currency.Code == dbAccount.Currency);

            if (correspondingCoinbaseAccount == null)
            {
                _logger.LogWarning($"Couldn't get account {dbAccount.Currency} from Coinbase API. Skipping.");
                return;
            }

            var exchangeRate = await _coinbaseApiConnector.GetExchangeRatesForCurrency(dbAccount.Currency);

            if (exchangeRate == null)
            {
                _logger.LogWarning($"Couldn't get exchange rates for {dbAccount.Currency} from Coinbase API. Skipping.");
                return;
            }

            var balance = correspondingCoinbaseAccount.Balance.Amount * exchangeRate.Rates[ExchangeRateConstants.NOK];

            var existingAsset = dbAccount.Assets.FirstOrDefault(x =>
                x.CreatedDate.Date == DateTime.Now.Date);

            if (existingAsset != null)
            {
                _logger.LogInformation($"Updating assets for {dbAccount.Currency}");

                existingAsset.Value = (int)balance;

                _dbRepository.QueueUpdate<Asset, AssetDto>(existingAsset);
            }
            else
            {
                _logger.LogInformation($"Adding assets for currency {dbAccount.Currency}");

                var asset = new AssetDto
                {
                    AccountId = dbAccount.Id,
                    Value = (int)balance
                };

                _dbRepository.QueueAdd<Asset, AssetDto>(asset);
            }
        }
    }
}