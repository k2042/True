using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace True.Gateway.Api.Configurations
{
    public static class CookieAuth
    {
        public static IHostApplicationBuilder RegisterCookieAuthentication(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            services.AddHttpContextAccessor();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.SlidingExpiration = true;

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };

                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    policy => policy
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                );
            });

            return builder;
        }
    }
}
