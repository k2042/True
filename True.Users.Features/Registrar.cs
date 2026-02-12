using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace True.Users.Features
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterUsersFeatures(this IHostApplicationBuilder builder)
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
