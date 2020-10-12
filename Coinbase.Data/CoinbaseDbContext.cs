using Coinbase.Data.Entities;
using Hub.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coinbase.Data
{
    public class CoinbaseDbContext : DbContext
    {
        public CoinbaseDbContext(DbContextOptions<CoinbaseDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .ToTable(schema: "dbo", name: "Account");

            builder.Entity<Asset>()
                .ToTable(schema: "dbo", name: "Asset");
            
            builder.AddSettingEntity();
            builder.AddWorkerLogEntity();
            builder.AddBackgroundTaskConfigurationEntity();
        }
    }
}