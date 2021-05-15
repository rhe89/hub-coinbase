using Hub.Storage.Repository.Entities;

namespace Coinbase.Core.Entities
{
    public class Asset : EntityBase
    {
        public long AccountId { get; set; }
        public int Value { get; set; }

        public virtual Account Account { get; set; }  
    }
}