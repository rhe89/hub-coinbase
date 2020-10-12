using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Coinbase.Providers;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountProvider _accountProvider;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountProvider accountProvider, ILogger<AccountController> logger)
        {
            _accountProvider = accountProvider;
            _logger = logger;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> Accounts()
        {
            _logger.LogInformation($"Request received at {DateTime.Now}");

            var accounts = await _accountProvider.GetAccounts();

            return Ok(accounts);
        }
    }
}
