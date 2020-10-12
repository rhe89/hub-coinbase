using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Coinbase.Data.Entities;
using Coinbase.Dto.Api;
using Hub.Storage.Repository;

namespace Coinbase.Providers
{
    public class AccountProvider : IAccountProvider
    {
        private readonly IDbRepository _dbRepository;

        public AccountProvider(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AccountDto>> GetAccounts()
        {
            var accounts = await _dbRepository.GetManyAsync<Account>(null, nameof(Account.Assets));

            var accountDtos = new List<AccountDto>();

            foreach (var account in accounts)
            {
                var balance = account.Assets
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();

                var lastMonthBalance = account.Assets
                    .Where(x => x.CreatedDate.Month == DateTime.Now.AddMonths(-1).Month
                                                                 && x.CreatedDate.Year ==
                                                                 DateTime.Now.AddMonths(-1).Year)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();
                
                var lastYearBalance = account.Assets
                    .Where(x => x.CreatedDate.Year ==
                                DateTime.Now.AddYears(-1).Year)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();

                accountDtos.Add(new AccountDto
                {
                    Name = account.Currency,
                    Balance = balance?.Value ?? 0,
                    LastMonthBalance = lastMonthBalance?.Value ?? 0,
                    LastYearBalance = lastYearBalance?.Value ?? 0,
                });
            }

            var total = new AccountDto
            {
                Name = "Total",
                Balance = accountDtos.Sum(x => x.Balance),
                LastMonthBalance = accountDtos.Sum(x => x.LastMonthBalance),
                LastYearBalance = accountDtos.Sum(x => x.LastYearBalance)
            };
            
            accountDtos.Add(total);

            return accountDtos;
        }
    }
}
