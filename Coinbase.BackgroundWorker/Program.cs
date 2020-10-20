using Microsoft.Extensions.Hosting;

namespace Coinbase.BackgroundWorker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new CoinbaseWorkerTimerHostBuilder(args).Build().Run();
        }
    }
}