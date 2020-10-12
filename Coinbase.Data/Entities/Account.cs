using System.Collections.Generic;
using System.Runtime.Serialization;
using Hub.Storage.Entities;

namespace Coinbase.Data.Entities
{
    [DataContract]
    public class Account : EntityBase
    {
        [DataMember]
        public string Currency { get; set; }

        public ICollection<Asset> Assets { get; set; }
    }
}
