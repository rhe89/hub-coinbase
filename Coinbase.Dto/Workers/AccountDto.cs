using System;

namespace Coinbase.Dto.Workers
{
    public class AccountDto
    {
        public string Currency { get; set; }
        public decimal Assets { get; set; }
        public decimal NativeBalance { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}