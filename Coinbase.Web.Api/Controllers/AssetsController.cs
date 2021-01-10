using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Coinbase.Web.Api.Services;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;

        public AssetsController(IAssetsService assetsService)
        {
            _assetsService = assetsService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Assets()
        {
            var assets = await _assetsService.GetAssets();

            return Ok(assets);
        }
    }
}
