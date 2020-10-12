using Coinbase.Data;
using Hub.Web.Startup;
using Microsoft.Extensions.Configuration;

namespace Coinbase.Web.Api
{
    public class Startup : ApiStartup<CoinbaseDbContext, DependencyRegistrationFactory>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
