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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UpdateAccountsTask> _logger;
        private readonly ICoinbaseConnector _coinbaseConnector;

        public UpdateAccountsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            IServiceScopeFactory serviceScopeFactory, 
            ILogger<UpdateAccountsTask> logger,
            ICoinbaseConnector coinbaseConnector) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _coinbaseConnector = coinbaseConnector;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            using var dbRepository = scope.ServiceProvider.GetRequiredService<IScopedHubDbRepository>();

            var accountsInDb = await dbRepository.AllAsync<Account, AccountDto>();

            var assets = await dbRepository.AllAsync<Asset, AssetDto>();

            var accountsCount = accountsInDb.Count();

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

                var existingValue = assets.FirstOrDefault(x => 
                    x.CreatedDate.Date == DateTime.Now.Date && 
                    x.AccountId == dbAccount.Id);

                if (existingValue != null)
                {
                    existingValue.Value = (int)correspondingCoinbaseAccount.NativeBalance;
                }
                else
                {
                    var value = new AssetDto
                    {
                        AccountId = dbAccount.Id,
                        Value = (int)correspondingCoinbaseAccount.NativeBalance
                    };

                    dbRepository.Add<Asset, AssetDto>(value);
                }
            }

            _logger.LogInformation($"Done updating cryptocurrencies.");

            await dbRepository.SaveChangesAsync();
        }
    }
}