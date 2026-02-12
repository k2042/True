using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Users.Infrastructure.Abstractions;
using True.Users.Infrastructure.Implementations;

namespace True.Users.Infrastructure
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterUsersInfrastructure(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddScoped<IUserRepository, UserRepository>();

            return builder;
        }
    }
}
