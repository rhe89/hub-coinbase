using System.Threading.Tasks;
using Coinbase.Core.Dto.Api;

namespace Coinbase.Web.Api.Services
{
    public interface IExchangeRatesService
    {
        Task<ExchangeRatesDto> GetExchangeRateForCurrency(string currency);
    }
}