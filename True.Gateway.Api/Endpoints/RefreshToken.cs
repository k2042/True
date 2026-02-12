using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using True.Common.Auth;
using True.User.Features.Commands;

namespace True.Gateway.Api.Endpoints
{
    public static class RefreshToken
    {
        public static void MapEndpoint(IEndpointRouteBuilder route)
        {
            route
                .MapGet("/refresh", Handler)
                .WithDescription("Get a frew JWT, authentication (Cookies) required")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .RequireAuthorization(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static async Task<IResult> Handler(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions,
            CancellationToken cancellationToken
        )
        {
            var userName = httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (userName == null)
            {
                return Results.BadRequest();
            }

            var token = TokenHandling.IssueTokenFor(userName);

            return Results.Ok(new { accessToken = token });

        }
    }
}
