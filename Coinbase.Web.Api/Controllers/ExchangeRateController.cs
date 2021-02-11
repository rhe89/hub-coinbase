using System.Threading.Tasks;
using Coinbase.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        public ExchangeRatesController(IExchangeRatesService exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService;
        }
        
        [HttpGet("exchangerates")]
        public async Task<IActionResult> ExchangeRates()
        {
            var assets = await _exchangeRatesService.GetExchangeRates();

            return Ok(assets);
        }
        
        [HttpGet("exchangerate")]
        public async Task<IActionResult> ExchangeRate(string currency)
        {
            var assets = await _exchangeRatesService.GetExchangeRateForCurrency(currency);

            return Ok(assets);
        }
    }
}