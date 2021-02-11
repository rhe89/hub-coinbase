namespace Coinbase.Core.Dto.Api
{
    public class ExchangeRatesDto
    {
        public string Currency { get; set; }
        public decimal NOKRate { get; set; }
        public decimal USDRate { get; set; }
        public decimal EURRate { get; set; }    
    }
}