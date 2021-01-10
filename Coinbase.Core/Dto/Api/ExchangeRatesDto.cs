using System.Collections.Generic;

namespace Coinbase.Core.Dto.Api
{
    public class ExchangeRatesDto
    {
        public string Currency { get; set; }
        public IDictionary<string, decimal> Rates { get; set; }
    }
}