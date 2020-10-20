using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Data.Entities;
using Coinbase.Dto.Workers;
using Coinbase.Integration;
using Hub.Storage.Repository;
using Hub.HostedServices.Tasks;
using Hub.Storage.Factories;
using Hub.Storage.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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

            using var dbRepository = scope.ServiceProvider.GetRequiredService<IScopedDbRepository>();

            var accountsInDb = await dbRepository.GetManyAsync<Account>();

            var assets = await dbRepository.GetManyAsync<Asset>();

            var accountsCount = accountsInDb.Count();

            var counter = 1;

            foreach (var dbAccount in accountsInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {accountsCount}: {dbAccount.Currency}.");

                var correspondingCoinbaseAccount = GetAccount(dbAccount.Currency);

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
                    var value = new Asset
                    {
                        AccountId = dbAccount.Id,
                        Value = (int)correspondingCoinbaseAccount.NativeBalance
                    };

                    dbRepository.Add(value);
                }
            }

            _logger.LogInformation($"Done updating cryptocurrencies.");

            await dbRepository.SaveChangesAsync();
        }
        
        private AccountDto GetAccount(string currency)
        {
            var account = _coinbaseConnector.GetAccountForCurrency(currency);

            if (account == null)
            {
                return null;
            }

            var nativeBalance = "";
            var balance = "";
            var createdDate = DateTime.MaxValue;

            if (account.Data.ExtraData.TryGetValue("native_balance", out var jTokenObject))
            {
                nativeBalance = jTokenObject["amount"].Value<string>();
            }
            
            if (account.Data.ExtraData.TryGetValue("balance", out jTokenObject))
            {
                balance = jTokenObject["amount"].Value<string>();
            }

            if (account.Data.ExtraData.TryGetValue("created_at", out jTokenObject))
            {
                DateTime.TryParse((string)jTokenObject, out createdDate);
            }

            var nativeBalanceParsed = decimal.Parse(nativeBalance, CultureInfo.InvariantCulture);
            var balanceParsed = decimal.Parse(balance, CultureInfo.InvariantCulture);

            return new AccountDto
            {
                Currency = currency,
                NativeBalance = nativeBalanceParsed,
                Assets = balanceParsed,
                CreatedDate = createdDate
            };
        }
    }
}