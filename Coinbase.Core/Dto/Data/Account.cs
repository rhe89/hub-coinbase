using System.Collections.Generic;
using Hub.Storage.Core.Dto;

namespace Coinbase.Core.Dto.Data
{
    public class AccountDto : EntityDtoBase
    {
        public string Currency { get; set; }

        public ICollection<AssetDto> Assets { get; set; }
    }
}
