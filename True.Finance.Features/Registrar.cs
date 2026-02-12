using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace True.Finance.Features
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterFinanceFeatures(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return builder;
        }
    }
}
