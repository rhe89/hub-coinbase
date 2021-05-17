using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;

namespace Coinbase.Core.Providers
{
    public interface IAccountBalanceProvider
    {
        Task<IList<AccountBalanceDto>> GetAssets(string accountName,
            DateTime? fromDate,
            DateTime? toDate);
    }
}