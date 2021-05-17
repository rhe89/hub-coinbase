using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Coinbase.Core.Providers;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountBalanceController : ControllerBase
    {
        private readonly IAccountBalanceProvider _accountBalanceProvider;

        public AccountBalanceController(IAccountBalanceProvider accountBalanceProvider)
        {
            _accountBalanceProvider = accountBalanceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountBalances([FromQuery]string accountName, 
            [FromQuery]DateTime? fromDate, 
            [FromQuery]DateTime? toDate)
        {
            var assets = await _accountBalanceProvider.GetAssets(accountName, fromDate, toDate);

            return Ok(assets);
        }
    }
}
