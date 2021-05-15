using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase.Core.Constants;
using Coinbase.Core.Exceptions;
using Coinbase.Core.Integration;
using Coinbase.Models;
using Hub.Settings.Core;

namespace Coinbase.Integration
{
    public class CoinbaseConnector : ICoinbaseConnector
    {
        private readonly CoinbaseClient _coinbaseClient;

        public CoinbaseConnector(ISettingProvider settingProvider)
        {
            var apiKey = settingProvider.GetSetting<string>(SettingConstants.CoinbaseApiKey);
            var apiSecret = settingProvider.GetSetting<string>(SettingConstants.CoinbaseApiSecret);
            _coinbaseClient = new CoinbaseClient(new ApiKeyConfig { ApiKey =  apiKey, ApiSecret = apiSecret});
        }
        
        public async Task<IList<Account>> GetAccounts()
        {
            PagedResponse<Account> response;

            try
            {
                response = await _coinbaseClient.Accounts.ListAccountsAsync();
            }
            catch (Exception e)
            {
                throw new CoinbaseApiConnectorException("CoinbaseConnector: Error sending request to Coinbase API", e);
            }

            ValidateResponse(response);
            
            if (response?.Data == null ||
                !response.Data.Any())
            {
                return null;
            }

            return response.Data.ToList();
        }
        
        public async Task<ExchangeRates> GetExchangeRatesForCurrency(string currency)
        {
            Response<ExchangeRates> response;

            try
            {
                response = await _coinbaseClient.Data.GetExchangeRatesAsync(currency);
            }
            catch (Exception e)
            {
                throw new CoinbaseApiConnectorException($"CoinbaseConnector: Error sending request to Coinbase API {e.Message}", e);
            }
            
            ValidateResponse(response);

            return response.Data;
        }
        
        private static void ValidateResponse(JsonResponse response)
        {
            if (response?.Errors == null || !response.Errors.Any() || response.Errors.Any(x => x.Id == "not_found"))
            {
                return;
            }
            
            var errors = response.Errors;

            var msg = string.Join(",", errors.Select(x => x.Message));

            throw new CoinbaseApiConnectorException($"CoinbaseConnector: Response from Coinbase contained errors: {msg}");
            
        }

    }
}
