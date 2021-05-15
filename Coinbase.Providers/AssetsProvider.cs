using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Providers;
using Hub.Storage.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace Coinbase.Providers
{
    public class AssetsProvider : IAssetsProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public AssetsProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AssetDto>> GetAssets()
        {
            var assets = await _dbRepository
                .AllAsync<Asset, AssetDto>(source => source.Include(x => x.Account));

            return assets;
        }
    }
}
