using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace True.Common.ServiceDefaults
{
    public static class ApiExtensions
    {
        public static IHostApplicationBuilder RegisterOpenApiServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder;
        }

        public static WebApplication AddOpenApiDocumentAndClient(this WebApplication app)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "openapi/{documentName}.json";
            });

            app.MapScalarApiReference();

            return app;
        }
    }
}
