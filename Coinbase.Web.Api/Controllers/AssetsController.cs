using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Coinbase.Web.Api.Services;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetsService assetsService, ILogger<AssetsController> logger)
        {
            _assetsService = assetsService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Assets()
        {
            _logger.LogInformation("Request received");

            var assets = await _assetsService.GetAssets();

            return Ok(assets);
        }
    }
}
