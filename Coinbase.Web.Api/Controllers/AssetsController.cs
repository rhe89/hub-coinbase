using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Coinbase.Providers;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetsProvider assetsProvider, ILogger<AssetsController> logger)
        {
            _assetsProvider = assetsProvider;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Assets()
        {
            _logger.LogInformation("Request received");

            var assets = await _assetsProvider.GetAssets();

            return Ok(assets);
        }
    }
}
