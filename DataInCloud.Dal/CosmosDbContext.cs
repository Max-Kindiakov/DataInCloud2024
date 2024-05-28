using DataInCloud.Dal.Shop;
using Microsoft.EntityFrameworkCore;

namespace DataInCloud.Dal
{
    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ShopDao> Shops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("max-kindiakov");
        }
    }
}
