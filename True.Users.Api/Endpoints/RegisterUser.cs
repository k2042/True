using MediatR;
using Microsoft.AspNetCore.Mvc;
using True.Users.Features.Commands;
using True.Users.Infrastructure.Abstractions;

namespace True.Users.Api.Endpoints
{
    public static class RegisterUser
    {
        public record RegisterRequest(string Name, string Password);

        public static void MapEndpointTo(IEndpointRouteBuilder route)
        {
            route
                .MapPost("/register", Handler)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .AllowAnonymous();
        }

        private static async Task<IResult> Handler(
            IMediator mediator,
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterUserCommand(
                new User(request.Name, request.Password)
            ));

            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
        }
    }
}