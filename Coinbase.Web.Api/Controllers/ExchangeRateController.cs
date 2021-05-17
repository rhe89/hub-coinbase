using System.Threading.Tasks;
using Coinbase.Core.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateProvider _exchangeRatesProvider;

        public ExchangeRatesController(IExchangeRateProvider exchangeRatesProvider)
        {
            _exchangeRatesProvider = exchangeRatesProvider;
        }
        
        public async Task<IActionResult> ExchangeRates([FromQuery]string currency)
        {
            var assets = await _exchangeRatesProvider.GetExchangeRates(currency);

            return Ok(assets);
        }
    }
}