using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using True.Finance.Features.Queries;

namespace True.Finance.Api.Endpoints
{
    public static class GetCurrencies
    {
        public static void MapEndpointTo(IEndpointRouteBuilder route)
        {
            route
                .MapGet("/", Handler)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Gets currency rates according to the passed favorites list, authentication (JWT Bearer) required")
                .RequireAuthorization(JwtBearerDefaults.AuthenticationScheme);
        }
         
        private static async Task<IResult> Handler(
            [FromQuery] string favorites,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var items = favorites.Split(",;|- ".ToArray());

            var result = await mediator.Send(new GetFavoriteCurrenciesQuery(items), cancellationToken);

            if (result.IsFailed)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Results.Ok(result.Value);
        }
    }
}