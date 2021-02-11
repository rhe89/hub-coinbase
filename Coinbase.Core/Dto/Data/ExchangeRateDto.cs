using Hub.Storage.Core.Dto;

namespace Coinbase.Core.Dto.Data
{
    public class ExchangeRateDto : EntityDtoBase
    {
        public string Currency { get; set; }
        public decimal NOKRate { get; set; }
        public decimal USDRate { get; set; }
        public decimal EURRate { get; set; }
    }
}