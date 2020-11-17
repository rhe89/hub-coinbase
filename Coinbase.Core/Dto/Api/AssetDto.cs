using System;

namespace Coinbase.Core.Dto.Api
{
    public class AssetDto
    {
        public string Currency { get; set; }
        public int Value { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
