using MediatR;
using Microsoft.AspNetCore.Mvc;
using True.Users.Features.Commands;

namespace True.Users.Api.Endpoints
{
    public static class LoginUser
    {
        public record LoginRequest(string Name, string Password);

        public static void MapEndpointTo(IEndpointRouteBuilder route)
        {
            route
                .MapPost("/login", Handler)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .AllowAnonymous();
        }

         
        private static async Task<IResult> Handler(
            IMediator mediator,
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new LoginUserCommand(request.Name, request.Password));

            return result.IsSuccess
                ? Results.Ok()
                : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
        }
    }
}