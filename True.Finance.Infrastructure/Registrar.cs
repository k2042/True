using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Finance.Infrastructure.Abstractions;
using True.Finance.Infrastructure.Implementations;

namespace True.Finance.Infrastructure
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterFinanceInfrastructure(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            return builder;
        }
    }
}
