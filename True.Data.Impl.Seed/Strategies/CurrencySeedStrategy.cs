using Microsoft.Extensions.Logging;
using True.Data.Model.Dbo;
using True.Data.Model.Seed;
using True.Data.Model;
using True.Data.Impl.Seed.Records;

namespace True.Data.Impl.Seed.Strategies
{
    public class CurrencySeedStrategy : BaseSeedStrategy<CurrencySeedRecord, ITrueDbContext>
    {
        public CurrencySeedStrategy(
            ISeedDataProvider<CurrencySeedRecord> seedDataProvider,
            ITrueDbContext dbContext,
            ILogger<CurrencySeedStrategy> logger)
            : base(seedDataProvider, dbContext, logger)
        {
        }

        protected override async Task SeedStrategyImplementation(CancellationToken cancellationToken)
        {
            if (DbContext.Currencies.Any())
            {
                Logger.LogInformation("Currencies are already presented in the database. Skipping seeding.");
                return;
            }

            var seedData = await SeedDataProvider.LoadSeedData(cancellationToken);

            DbContext.Currencies.AddRange(seedData.Select(record => new CurrencyDbo
            {
                Id = record.Id,
                Name = record.Name,
                Rate = record.Rate
            }));

            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
