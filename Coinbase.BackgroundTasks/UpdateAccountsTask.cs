using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Integration;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Hub.Storage.Core.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            var accountsInDb = await _dbRepository.AllAsync<Account, AccountDto>();
            
            var assets = await _dbRepository.AllAsync<Asset, AssetDto>();

            var accountsCount = accountsInDb.Count;

            var counter = 1;

            foreach (var dbAccount in accountsInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {accountsCount}: {dbAccount.Currency}.");

                var correspondingCoinbaseAccount = _coinbaseConnector.GetAccountForCurrency(dbAccount.Currency);

                if (correspondingCoinbaseAccount == null)
                {
                    _logger.LogInformation($"Couldn't get account {dbAccount.Currency} from Coinbase. Skipping.");
                    continue;
                }

                if (correspondingCoinbaseAccount.NativeBalance == 0)
                {
                    _logger.LogInformation($"Assets is 0. Skipping account {dbAccount.Currency}.");
                    continue;
                }

                var existingAsset = assets.FirstOrDefault(x => 
                    x.CreatedDate.Date == DateTime.Now.Date && 
                    x.AccountId == dbAccount.Id);

                if (existingAsset != null)
                {
                    
                    _logger.LogInformation($"Updating assets for currency {dbAccount.Currency}");

                    existingAsset.Value = (int)correspondingCoinbaseAccount.NativeBalance;
                    
                    _dbRepository.Update<Asset, AssetDto>(existingAsset);
                }
                else
                {
                    _logger.LogInformation($"Adding assets for currency {dbAccount.Currency}");

                    var asset = new AssetDto
                    {
                        AccountId = dbAccount.Id,
                        Value = (int)correspondingCoinbaseAccount.NativeBalance
                    };

                    _dbRepository.Add<Asset, AssetDto>(asset);
                }
            }

            _logger.LogInformation($"Done updating cryptocurrencies.");
            
            _dbRepository.ToggleDispose(true);

            await _dbRepository.SaveChangesAsync();
        }
    }
}