using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;

namespace Coinbase.Core.Providers
{
    public interface IAssetsProvider
    {
        Task<IList<AssetDto>> GetAssets();
    }
}