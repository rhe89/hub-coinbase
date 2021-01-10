using System.Threading.Tasks;
using Coinbase.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly ILogger<ExchangeRatesController> _logger;

        public ExchangeRatesController(IExchangeRatesService exchangeRatesService,
            ILogger<ExchangeRatesController> logger)
        {
            _exchangeRatesService = exchangeRatesService;
            _logger = logger;
        }
        
        [HttpGet("exchangerates")]
        public async Task<IActionResult> Assets(string currency)
        {
            var assets = await _exchangeRatesService.GetExchangeRateForCurrency(currency);

            return Ok(assets);
        }
    }
}