using Hub.Storage.Repository.Dto;

namespace Coinbase.Core.Dto.Data
{
    public class AccountBalanceDto : EntityDtoBase
    {
        public long AccountId { get; set; }
        public decimal Balance { get; set; }
    }
}