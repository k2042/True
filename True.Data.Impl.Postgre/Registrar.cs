using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using True.Data.Model;

namespace True.Data.Impl.Postgre
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterDataImplementationPostgre(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<TrueDbContextPostgre>((provider, options) => options.BuildOptions(provider, appsettings))
                .AddScoped<ITrueDbContext>(provider => provider.GetRequiredService<TrueDbContextPostgre>());

            return builder;
        }

        private static void BuildOptions(this DbContextOptionsBuilder options, IServiceProvider provider, IConfiguration appsettings)
        {
            var (dbProvider, connectionString) = appsettings.ExtractParams();

            options
                .UseNpgsql(connectionString)
                .UseInternalServiceProvider(provider);
        }

        private static (string dbProvider, string connectionString) ExtractParams(this IConfiguration appsettings)
        {
            var dbProvider = appsettings["DatabaseProvider"]
                ?? throw new ArgumentNullException("Need to specify [\"DatabaseProvider\"] at the root of appsettings");
            var connectionString = appsettings.GetConnectionString(dbProvider)
                ?? throw new ArgumentNullException($"Need to specify [\"{dbProvider}\"] connection string in the [\"ConnectionStrings\"] section of appsettings");

            return (dbProvider, connectionString);
        }

        public static async Task ApplyMigrations(this AsyncServiceScope scope, CancellationToken cancellationToken)
        {
            await using var context = scope.ServiceProvider.GetRequiredService<ITrueDbContext>();
            await context.Database.MigrateAsync(cancellationToken);
        }
    }
}
