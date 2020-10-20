using Coinbase.ObjectModel;
using System;
using System.Linq;
using Coinbase.Constants;
using Hub.Storage.Providers;

namespace Coinbase.Integration
{
    public class CoinbaseConnector : ICoinbaseConnector
    {
        private readonly CoinbaseApi _coinbaseApi;

        public CoinbaseConnector(ISettingProvider settingProvider)
        {
            var apiKey = settingProvider.GetSetting<string>(SettingConstants.CoinbaseApiKey);
            var apiSecret = settingProvider.GetSetting<string>(SettingConstants.CoinbaseApiSecret);
            _coinbaseApi = new CoinbaseApi(apiKey, apiSecret, false);
        }

        public CoinbaseResponse<CoinbaseResponse> GetAccountForCurrency(string currency)
        {
            CoinbaseResponse<CoinbaseResponse> response;

            try
            {
                response = _coinbaseApi.SendGetRequest<CoinbaseResponse>($"accounts/{currency}", null);

            }
            catch (Exception e)
            {
                throw new CoinbaseConnectorException("CoinbaseConnector: Error sending request to Coinbase API", e);
            }

            if (response?.Errors != null &&
                response.Errors.Any() && 
                response.Errors.All(x => x.Id != "not_found"))
            {
                var errors = response.Errors;

                var msg = string.Join(",", errors.Select(x => x.Message));

                throw new CoinbaseConnectorException($"CoinbaseConnector: Response from Coinbase contained errors: {msg}");
            }


            if (response?.Data?.ExtraData == null ||
                !response.Data.ExtraData.Any())
            {
                return null;
            }

            return response;
        }
    }
}
