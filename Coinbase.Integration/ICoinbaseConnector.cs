using Coinbase.ObjectModel;

namespace Coinbase.Integration
{
    public interface ICoinbaseConnector
    {
        CoinbaseResponse<CoinbaseResponse> GetAccountForCurrency(string currency);
    }
}