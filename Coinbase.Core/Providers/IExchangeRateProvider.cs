using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;

namespace Coinbase.Core.Providers
{
    public interface IExchangeRateProvider
    {
        Task<IList<ExchangeRateDto>> GetExchangeRates();
        Task<ExchangeRateDto> GetExchangeRate(string currency);
    }
}