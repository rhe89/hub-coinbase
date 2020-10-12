using Coinbase.Data;
using Hub.Web.HostBuilder;
using Microsoft.Extensions.Hosting;

namespace Coinbase.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return ApiHostBuilder.CreateHostBuilder<Startup, DependencyRegistrationFactory, CoinbaseDbContext>(args);
        }
    }
}
