using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Integration.Cbr.Infrastructure.Currency.Client;

namespace True.Gateway.Infrastructure
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterCbrClients(this IHostApplicationBuilder builder)        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            services
                .AddHttpClient<CurrenciesApiClient>(client => client.Configure<CurrenciesApiClient>(appsettings))
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
