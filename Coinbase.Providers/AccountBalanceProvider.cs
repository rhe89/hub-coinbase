﻿using System;
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
    public class AccountBalanceProvider : IAccountBalanceProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public AccountBalanceProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AccountBalanceDto>> GetAssets(string accountName, 
            DateTime? fromDate,
            DateTime? toDate)
        {
            Expression<Func<AccountBalance, bool>> predicate = accountBalance => 
                (string.IsNullOrEmpty(accountName) || accountBalance.Account.Currency.ToLower().Contains(accountName.ToLower()))
                    && (fromDate == null || accountBalance.CreatedDate >= fromDate.Value)
                    && (toDate == null || accountBalance.CreatedDate <= toDate.Value);

            var accountBalances = await _dbRepository
                .WhereAsync<AccountBalance, AccountBalanceDto>(predicate, queryable => queryable.Include(x => x.Account));

            return accountBalances;
        }
    }
}
