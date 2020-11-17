using Coinbase.ObjectModel;
using System;
using System.Globalization;
using System.Linq;
using Coinbase.Core.Constants;
using Coinbase.Core.Dto.Integration;
using Coinbase.Core.Exceptions;
using Coinbase.Core.Integration;
using Hub.Storage.Core.Providers;
using Newtonsoft.Json.Linq;

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

        public CoinbaseAccount GetAccountForCurrency(string currency)
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

            return MapAccount(response, currency);
        }

        private CoinbaseAccount MapAccount(CoinbaseResponse<CoinbaseResponse> coinbaseResponse, string currency)
        {
            if (coinbaseResponse == null)
            {
                return null;
            }

            var nativeBalance = "";
            var balance = "";
            var createdDate = DateTime.MaxValue;

            if (coinbaseResponse.Data.ExtraData.TryGetValue("native_balance", out var jTokenObject))
            {
                nativeBalance = jTokenObject["amount"].Value<string>();
            }
            
            if (coinbaseResponse.Data.ExtraData.TryGetValue("balance", out jTokenObject))
            {
                balance = jTokenObject["amount"].Value<string>();
            }

            if (coinbaseResponse.Data.ExtraData.TryGetValue("created_at", out jTokenObject))
            {
                DateTime.TryParse((string)jTokenObject, out createdDate);
            }

            var nativeBalanceParsed = decimal.Parse(nativeBalance, CultureInfo.InvariantCulture);
            var balanceParsed = decimal.Parse(balance, CultureInfo.InvariantCulture);

            return new CoinbaseAccount
            {
                Currency = currency,
                NativeBalance = nativeBalanceParsed,
                Assets = balanceParsed,
                CreatedDate = createdDate
            };
        }
    }
}
