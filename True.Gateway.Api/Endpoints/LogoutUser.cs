using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace True.Gateway.Api.Endpoints
{
    public static class LogoutUser
    {
        public static void MapEndpoint(IEndpointRouteBuilder route)
        {
            route
                .MapPost("/logout", Logout)
                .WithDescription("Logout a user (invalidate cookie), authentication (Cookies) required")
                .RequireAuthorization(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static async Task<IResult> Logout(
            IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken
        )
        {
            if (httpContextAccessor.HttpContext != null)
            {
                await httpContextAccessor.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                return Results.Ok();
            }

            return Results.BadRequest();
        }
    }
}
