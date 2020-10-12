using Microsoft.Extensions.Hosting;

namespace Coinbase.BackgroundWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new CoinbaseWorkerTimerHostBuilder(args).Build().Run();
        }
    }
}