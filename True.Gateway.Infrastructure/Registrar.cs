using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Gateway.Infrastructure.Finance.Abstractions;
using True.Gateway.Infrastructure.Finance.Implementations;
using True.Gateway.Infrastructure.User.Abstractions;
using True.Gateway.Infrastructure.User.Implementations;

namespace True.Gateway.Infrastructure
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterGatewayInfrastructure(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add("Authorization");
                options.Headers.Add("X-Correlation-ID");
            });

            services
                .AddHttpClient<IUsersApiClient, UsersApiClient>(client => client.Configure<UsersApiClient>(appsettings))
                .AddHeaderPropagation()
                .AddStandardResilienceHandler();

            services
                .AddHttpClient<IFinanceApiClient, FinanceApiClient>(client => client.Configure<FinanceApiClient>(appsettings))
                .AddHeaderPropagation()
                .AddStandardResilienceHandler();

            return builder;
        }

        private static void Configure<TClient>(this HttpClient client, IConfiguration appsettings)
            where TClient : class
        {
            var typeName = typeof(TClient).Name;
            var section = appsettings.GetSection(typeName)
                ?? throw new NullReferenceException($"{{{typeName}}} configuration section is missing");
            var config = section.Get<HttpClientConfig>()
                ?? throw new NullReferenceException($"Error populating {{{nameof(HttpClientConfig)}}}");

            client.BaseAddress = new Uri(config.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
        }

        private class HttpClientConfig
        {
            public string BaseAddress { get; set; } = null!;
            public int TimeoutSeconds { get; set; }
        }
    }
}
