using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace True.Data.Impl.Postgre
{
    public class TrueDbContextFactory : IDesignTimeDbContextFactory<TrueDbContextPostgre>
    {
        public TrueDbContextPostgre CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<TrueDbContextPostgre>()
                .UseNpgsql("Host=localhost;Database=true_db;Username=true_user;Password=true_password")
                .Options;

            return new TrueDbContextPostgre(options);
        }
    }
}
