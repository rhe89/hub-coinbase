using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Api;

namespace Coinbase.Web.Api.Services
{
    public interface IAccountService
    {
        Task<IList<AccountDto>> GetAccounts();
    }
}