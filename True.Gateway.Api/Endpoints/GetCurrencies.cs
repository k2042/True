using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using True.Gateway.Features.Finance.Queries;

namespace True.Gateway.Api.Endpoints
{
    public static class GetCurrencies
    {
        public static void MapEndpoint(IEndpointRouteBuilder route)
        {
            route
                .MapGet("/", Handler)
                .WithDescription("Gets currency rates according to the passed favorites list, authentication (JWT Bearer) required")
                .RequireAuthorization(JwtBearerDefaults.AuthenticationScheme);
        }

        public static async Task<IResult> Handler(
            [FromQuery] string? favorites,
            IMediator mediator,
            CancellationToken cancellationToken
        )
        {
            if (favorites == null)
            {
                return Results.BadRequest();
            }

            var items = favorites.Split(",;|- ".ToArray());

            if (items.Length == 0)
            {
                return Results.BadRequest();
            }

            var result = await mediator.Send(new GetFavoriteCurrenciesQuery(items), cancellationToken);

            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }

            return Results.StatusCode(StatusCodes.Status500InternalServerError);

        }
    }
}
