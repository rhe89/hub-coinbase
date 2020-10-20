using Coinbase.Data;
using Hub.Web.HostBuilder;
using Microsoft.Extensions.Hosting;

namespace Coinbase.Web.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new ApiHostBuilder<Startup, DependencyRegistrationFactory, CoinbaseDbContext>().CreateHostBuilder(args);
        }
    }
}
