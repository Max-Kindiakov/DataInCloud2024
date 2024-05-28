using DataInCloud.Dal.Car;
using Microsoft.EntityFrameworkCore;

namespace DataInCloud.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<CarDao> Cars { get; set; }
    }
}
