using Microsoft.Extensions.Logging;
using True.Data.Model.Dbo;
using True.Data.Model.Seed;
using True.Data.Model;
using True.Data.Impl.Seed.Records;

namespace True.Data.Impl.Seed.Strategies
{
    public class UserSeedStrategy : BaseSeedStrategy<UserSeedRecord, ITrueDbContext>
    {
        public UserSeedStrategy(
            ISeedDataProvider<UserSeedRecord> seedDataProvider,
            ITrueDbContext dbContext,
            ILogger<UserSeedStrategy> logger)
            : base(seedDataProvider, dbContext, logger)
        {
        }

        protected override async Task SeedStrategyImplementation(CancellationToken cancellationToken)
        {
            if (DbContext.Users.Any())
            {
                Logger.LogInformation("Users are already presented in the database. Skipping seeding.");
                return;
            }

            var seedData = await SeedDataProvider.LoadSeedData(cancellationToken);

            DbContext.Users.AddRange(seedData.Select(record => new UserDbo
            {
                Name = record.Name,
                Password = record.Password
            }));

            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
