using Coinbase.Data;
using Hub.Web.Api;
using Microsoft.Extensions.Hosting;

namespace Coinbase.Web.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return HostBuilder<DependencyRegistrationFactory, CoinbaseDbContext>
                .Create(args);

        }
    }
}
