using Microsoft.EntityFrameworkCore;
using True.Data.Model;
using True.Data.Impl.Postgre.Configurations;

namespace True.Data.Impl.Postgre
{
    public class TrueDbContextPostgre(DbContextOptions options) : TrueDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
