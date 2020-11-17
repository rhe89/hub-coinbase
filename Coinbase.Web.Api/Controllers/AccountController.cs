using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Coinbase.Web.Api.Services;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> Accounts()
        {
            _logger.LogInformation($"Request received at {DateTime.Now}");

            var accounts = await _accountService.GetAccounts();

            return Ok(accounts);
        }
    }
}
