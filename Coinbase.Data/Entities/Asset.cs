using System.Runtime.Serialization;
using Hub.Storage.Entities;

namespace Coinbase.Data.Entities
{
    [DataContract]
    public class Asset : EntityBase
    {
        [DataMember]
        public long AccountId { get; set; }
        [DataMember]
        public int Value { get; set; }

        public Account Account { get; set; }  
    }
}