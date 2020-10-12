using System;

namespace Coinbase.Dto.Api
{
    public class AssetDto
    {
        public string Currency { get; set; }
        public int Value { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
