using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase.Data.Entities;
using Coinbase.Dto.Api;
using Hub.Storage.Repository;

namespace Coinbase.Providers
{
    public class AssetsProvider : IAssetsProvider
    {
        private readonly IDbRepository _dbRepository;

        public AssetsProvider(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AssetDto>> GetAssets()
        {
            var assets = await _dbRepository.GetManyAsync<Asset>(null, nameof(Asset.Account));

            return assets
                .Select(x => new AssetDto
                {
                    Currency = x.Account.Currency,
                    Value = x.Value,
                    CreatedDate = x.CreatedDate
                })
                .ToList();
        }
    }
}
