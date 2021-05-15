using System.Collections.Generic;
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

        public async Task<IList<AccountDto>> GetAccounts()
        {
            var accounts = await _dbRepository
                .AllAsync<Account, AccountDto>(source => source.Include(x => x.Assets));

            return accounts;


        }
    }
}
