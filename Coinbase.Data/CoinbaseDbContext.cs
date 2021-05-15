using Coinbase.Core.Entities;
using Hub.Storage.Repository;
using Microsoft.EntityFrameworkCore;

namespace Coinbase.Data
{
    public class CoinbaseDbContext : HubDbContext
    {
        public CoinbaseDbContext(DbContextOptions<CoinbaseDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Account>()
                .ToTable(schema: "dbo", name: "Account");

            builder.Entity<Asset>()
                .ToTable(schema: "dbo", name: "Asset");
            
            builder.Entity<ExchangeRate>()
                .ToTable(schema: "dbo", name: "ExchangeRate");
        }
    }
}