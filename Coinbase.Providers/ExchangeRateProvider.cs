using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Providers;
using Hub.Storage.Repository.Core;

namespace Coinbase.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public ExchangeRateProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        
        public async Task<IList<ExchangeRateDto>> GetExchangeRates(string currency)
        {
            Expression<Func<ExchangeRate, bool>> predicate = exchangeRate =>
                (string.IsNullOrEmpty(currency) || exchangeRate.Currency.ToLower().Contains(currency.ToLower()));
                
            return await _dbRepository.WhereAsync<ExchangeRate, ExchangeRateDto>(predicate);
        }
    }
}