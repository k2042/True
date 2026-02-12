using Microsoft.EntityFrameworkCore;
using True.Data.Model.Dbo;

namespace True.Data.Model
{
    public abstract class TrueDbContext(DbContextOptions options) : DbContext(options), ITrueDbContext
    {
        public DbSet<UserDbo> Users { get; set; }

        public DbSet<CurrencyDbo> Currencies { get; set; }
    }
}
