using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;

namespace Coinbase.Web.Api.Services
{
    public interface IExchangeRatesService
    {
        Task<IList<ExchangeRateDto>> GetExchangeRates();
        Task<ExchangeRateDto> GetExchangeRateForCurrency(string currency);
    }
}