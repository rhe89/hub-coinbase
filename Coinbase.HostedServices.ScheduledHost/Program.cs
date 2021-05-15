using Coinbase.Data;
using Hub.HostedServices.Schedule;
using Microsoft.Extensions.Hosting;

namespace Coinbase.HostedServices.ScheduledHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new Bootstrapper<DependencyRegistrationFactory, CoinbaseDbContext>(args,"SQL_DB_COINBASE")
                .CreateHostBuilder()
                .Build()
                .Run();
        }
    }
}