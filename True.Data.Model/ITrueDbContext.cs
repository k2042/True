using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using True.Data.Model.Dbo;

namespace True.Data.Model
{
    public interface ITrueDbContext : IAsyncDisposable, IDisposable
    {
        DatabaseFacade Database { get; }
        DbSet<UserDbo> Users { get; set; }
        DbSet<CurrencyDbo> Currencies { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
