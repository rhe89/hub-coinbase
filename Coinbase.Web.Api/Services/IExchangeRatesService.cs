using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Api;

namespace Coinbase.Web.Api.Services
{
    public interface IExchangeRatesService
    {
        Task<IList<ExchangeRatesDto>> GetExchangeRates();
        Task<ExchangeRatesDto> GetExchangeRateForCurrency(string currency);
    }
}