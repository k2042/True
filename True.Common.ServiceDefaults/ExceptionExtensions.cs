using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using True.Common.ServiceDefaults.Exceptions;

namespace True.Common.ServiceDefaults
{
    public static class ExceptionExtensions
    {
        public static IHostApplicationBuilder RegisterCommonExceptionHandler(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;

            builder.Services.AddExceptionHandler<CommonExceptionHandler>();
            builder.Services.AddProblemDetails();

            return builder;
        }
    }
}
