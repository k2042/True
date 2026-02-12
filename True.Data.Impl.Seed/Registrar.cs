using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Data.Impl.Seed.Loading;
using True.Data.Impl.Seed.Records;
using True.Data.Impl.Seed.Strategies;
using True.Data.Model.Seed;

namespace True.Data.Impl.Seed
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterDataImplementationSeed(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            services.AddScoped<ISeedDataProvider<UserSeedRecord>>(_ => new CsvDataProvider<UserSeedRecord>("Data\\Users.csv"));
            services.AddScoped<ISeedDataProvider<CurrencySeedRecord>>(_ => new CsvDataProvider<CurrencySeedRecord>("Data\\Currencies.csv"));

            services.AddScoped<ISeedStrategy, UserSeedStrategy>();
            services.AddScoped<ISeedStrategy, CurrencySeedStrategy>();

            return builder;
        }
    }
}
