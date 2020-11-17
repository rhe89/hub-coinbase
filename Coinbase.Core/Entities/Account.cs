using System.Collections.Generic;
using Hub.Storage.Core.Entities;

namespace Coinbase.Core.Entities
{
    public class Account : EntityBase
    {
        public string Currency { get; set; }

        public ICollection<Asset> Assets { get; set; }
    }
}
