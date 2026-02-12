using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Filters;

namespace True.Common.ServiceDefaults
{
    public static class LogExtensions
    {
        public static ILogger CreateBootstrapLogger(string filePath = "init.log")
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry()
                .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();
        }

        public static IHostApplicationBuilder RegisterRuntimeLogger(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var filePath = "server.log";

            var includedRequests = Matching.WithProperty<string>(
                "RequestPath",
                path => path.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
            );

            var loggerConfiguration = new LoggerConfiguration()
                //.ReadFrom.Configuration(appsettings)
                //.Filter.ByIncludingOnly(includedRequests)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry()
                .WriteTo.File(filePath, rollingInterval: RollingInterval.Day);

            Log.Logger = loggerConfiguration.CreateLogger();

            builder.Services.AddSerilog();

            return builder;
        }
    }
}
