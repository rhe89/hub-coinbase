using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Providers;
using Hub.Storage.Core.Repository;

namespace Coinbase.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public ExchangeRateProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        
        public async Task<IList<ExchangeRateDto>> GetExchangeRates()
        {
            return await _dbRepository.AllAsync<ExchangeRate, ExchangeRateDto>();
        }
        
        public async Task<ExchangeRateDto> GetExchangeRate(string currency)
        {
            return await _dbRepository.FirstOrDefaultAsync<ExchangeRate, ExchangeRateDto>(er => er.Currency == currency);
        }
    }
}