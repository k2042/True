using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;
using True.Common.Auth;
using True.User.Features.Commands;

namespace True.Gateway.Api.Endpoints
{
    public static class LoginUser
    {
        public record LoginRequest(string Name, string Password);

        public static void MapEndpoint(IEndpointRouteBuilder route)
        {
            route
                .MapPost("/login", Handler)
                .Accepts<LoginRequest>(MediaTypeNames.Application.FormUrlEncoded)
                .WithDescription("Login user (set cookies, receive a JWT), no authentication required")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        private static async Task<IResult> Handler(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            [FromForm] LoginRequest request,
            CancellationToken cancellationToken
        )
        {
            var result = await mediator.Send(new LoginUserCommand(
                request.Name,
                request.Password
            ));

            if (!result.IsSuccess || httpContextAccessor.HttpContext == null)
            {
                return Results.Unauthorized();
            }

            await IssueCookiesFor(httpContextAccessor.HttpContext, request.Name);

            var token = TokenHandling.IssueTokenFor(request.Name);

            return Results.Ok(new { accessToken = token });
        }

        private static async Task IssueCookiesFor(HttpContext httpContext, string userName, IEnumerable<Claim>? additionalClaims = null)
        {
            List<Claim> claims =
            [
                new (ClaimTypes.Name, userName)
            ];

            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                properties
            );
        }
    }
}
