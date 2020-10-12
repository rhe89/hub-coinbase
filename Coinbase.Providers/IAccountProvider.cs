using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Dto.Api;

namespace Coinbase.Providers
{
    public interface IAccountProvider
    {
        Task<IList<AccountDto>> GetAccounts();
    }
}