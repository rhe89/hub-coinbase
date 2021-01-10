using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Models;

namespace Coinbase.Core.Integration
{
    public interface ICoinbaseConnector
    {
        Task<IList<Account>> GetAccounts();
        Task<ExchangeRates> GetExchangeRatesForCurrency(string currency);
    }
}