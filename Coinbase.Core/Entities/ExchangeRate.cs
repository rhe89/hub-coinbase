using Hub.Storage.Core.Entities;

namespace Coinbase.Core.Entities
{
    public class ExchangeRate : EntityBase
    {
        public string Currency { get; set; }
        public decimal NOKRate { get; set; }
        public decimal USDRate { get; set; }
        public decimal EURRate { get; set; }
    }
}