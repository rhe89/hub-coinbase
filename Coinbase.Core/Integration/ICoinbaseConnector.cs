using Coinbase.Core.Dto.Integration;

namespace Coinbase.Core.Integration
{
    public interface ICoinbaseConnector
    {
        CoinbaseAccount GetAccountForCurrency(string currency);
    }
}