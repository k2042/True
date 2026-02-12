using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace True.Gateway.Features
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterIntegrationCbrHandlers(this IHostApplicationBuilder builder)
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
