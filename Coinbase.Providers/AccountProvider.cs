using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Providers;
using Hub.Storage.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace Coinbase.Providers
{
    public class AccountProvider : IAccountProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public AccountProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AccountDto>> GetAccounts(string accountName)
        {
            Expression<Func<Account, bool>> predicate = account =>
                (string.IsNullOrEmpty(accountName) || account.Currency.ToLower().Contains(accountName.ToLower()));
                
            var accounts = await _dbRepository
                .WhereAsync<Account, AccountDto>(predicate);

            return accounts;


        }
    }
}
