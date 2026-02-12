using Microsoft.Extensions.Logging;

namespace True.Data.Model.Seed
{
    public abstract class BaseSeedStrategy<TSeedRecord, TDbContext> : ISeedStrategy
        where TSeedRecord : class
        where TDbContext : ITrueDbContext
    {
        protected readonly ISeedDataProvider<TSeedRecord> SeedDataProvider;
        protected readonly TDbContext DbContext;
        protected readonly ILogger<BaseSeedStrategy<TSeedRecord, TDbContext>> Logger;

        public BaseSeedStrategy(
            ISeedDataProvider<TSeedRecord> seedDataProvider,
            TDbContext dbContext,
            ILogger<BaseSeedStrategy<TSeedRecord, TDbContext>> logger
        )
        {
            SeedDataProvider = seedDataProvider;
            DbContext = dbContext;
            Logger = logger;
        }

        public async Task SeedData(CancellationToken cancellationToken)
        {
            var dataTypeName = typeof(TSeedRecord).Name;

            Logger.LogInformation("Commencing import: {{{name}}}", dataTypeName);

            try
            {
                await SeedStrategyImplementation(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Fiasco importing: {{{name}}}", dataTypeName);
            }

            Logger.LogInformation("Consummated import: {{{name}}}", dataTypeName);
        }

        protected abstract Task SeedStrategyImplementation(CancellationToken cancellationToken);
    }
}
