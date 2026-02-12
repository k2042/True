using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Data.Model;
using True.Data.Model.Seed;

namespace True.Data.Impl.Migrator.Background
{
    public class Worker : BackgroundService
    {
        private readonly ITrueDbContext _dbContext;
        private readonly IHostApplicationLifetime _appLifetime;

        public Worker(
            ITrueDbContext dbContext,
            IHostApplicationLifetime appLifetime
        )
        {
            _dbContext = dbContext;
            _appLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);

            var services = _dbContext.Database.GetService<IServiceProvider>();
            var strategies = services.GetServices<ISeedStrategy>();

            foreach (var strategy in strategies)
            {
                await strategy.SeedData(cancellationToken);
            }

            _appLifetime.StopApplication();
        }
    }
}
