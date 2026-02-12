using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace True.Common.Auth
{
    public static class Registrar
    {
        public static IHostApplicationBuilder RegisterTokenAuthentication(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            TokenHandling.JwtOptionsInstance = appsettings
                .GetSection(nameof(JwtOptions))
                .Get<JwtOptions>() ?? throw new NullReferenceException($"Add {{{nameof(JwtOptions)}}} in appsettings");

            services.AddSingleton(Options.Create(TokenHandling.JwtOptionsInstance));

            services
                .AddAuthentication(static options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(static options =>
                {
                    options.TokenValidationParameters.ValidIssuer = TokenHandling.JwtOptionsInstance.Issuer;
                    options.TokenValidationParameters.ValidAudience = TokenHandling.JwtOptionsInstance.Audience;
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(TokenHandling.JwtOptionsInstance.SecretKey)
                    );
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    JwtBearerDefaults.AuthenticationScheme,
                    policy => policy
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                );
            });

            return builder;
        }
    }
}
