using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Providers;
using Hub.Storage.Core.Repository;
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
            var accounts = _dbRepository
                .Set<Account>()
                .Include(x => x.Assets);

            return await _dbRepository.ProjectAsync<Account, AccountDto>(accounts);


        }
    }
}
