using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Coinbase.Core.Providers;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountProvider _accountProvider;

        public AccountsController(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Accounts([FromQuery]string accountName)
        {
            var accounts = await _accountProvider.GetAccounts(accountName);

            return Ok(accounts);
        }
    }
}
