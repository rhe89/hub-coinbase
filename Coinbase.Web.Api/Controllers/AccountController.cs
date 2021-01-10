using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Coinbase.Web.Api.Services;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _accountService.GetAccounts();

            return Ok(accounts);
        }
    }
}
