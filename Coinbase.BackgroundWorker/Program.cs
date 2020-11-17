using Coinbase.Data;
using Hub.HostedServices.TimerHost;
using Microsoft.Extensions.Hosting;

namespace Coinbase.BackgroundWorker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new BackgroundWorker<DependencyRegistrationFactory, CoinbaseDbContext>(args,"SQL_DB_COINBASE")
                .CreateHostBuilder()
                .Build()
                .Run();
        }
    }
}