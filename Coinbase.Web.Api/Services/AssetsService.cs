using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Api;
using Coinbase.Core.Providers;

namespace Coinbase.Web.Api.Services
{
    public class AssetsService : IAssetsService
    {
        private readonly IAssetsProvider _assetsProvider;

        public AssetsService(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public async Task<IList<AssetDto>> GetAssets()
        {
            var assets = await _assetsProvider.GetAssets();
            
            return assets
                .Select(x => new AssetDto
                {
                    Currency = x.Account?.Currency,
                    Value = x.Value,
                    CreatedDate = x.CreatedDate
                })
                .ToList();
        }
    }
}
