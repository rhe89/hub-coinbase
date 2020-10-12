using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Dto.Api;

namespace Coinbase.Providers
{
    public interface IAssetsProvider
    {
        Task<IList<AssetDto>> GetAssets();
    }
}