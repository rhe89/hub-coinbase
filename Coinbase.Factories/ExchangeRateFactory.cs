using Coinbase.Core.Dto.Data;
using Coinbase.Core.Entities;
using Coinbase.Core.Factories;
using Hub.Storage.Repository.Core;

namespace Coinbase.Factories
{
    public class ExchangeRateFactory : IExchangeRateFactory
    {
        private readonly IHubDbRepository _dbRepository;

        public ExchangeRateFactory(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        
        public ExchangeRateDto Add(string currency, decimal nokRate, decimal usdRate, decimal eurRate)
        {
            var exchangeRateDto = new ExchangeRateDto
            {
                Currency = currency,
                NOKRate = nokRate,
                USDRate = usdRate,
                EURRate = eurRate
            };

            return _dbRepository.Add<ExchangeRate, ExchangeRateDto>(exchangeRateDto);
        }
    }
}