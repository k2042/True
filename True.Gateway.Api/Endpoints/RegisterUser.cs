using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using True.User.Features.Commands;

namespace True.Gateway.Api.Endpoints
{
    public static class RegisterUser
    {
        public record RegisterRequest(string Name, string Password);

        public static void MapEndpoint(IEndpointRouteBuilder route)
        {
            route
                .MapPost("/register", Handler)
                .Accepts<RegisterRequest>(MediaTypeNames.Application.FormUrlEncoded)
                .WithDescription("Register a new user, no authentication required")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> Handler(
            IMediator mediator,
            [FromForm] RegisterRequest form,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterUserCommand(form.Name, form.Password));

            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return Results.BadRequest(result.Errors.FirstOrDefault()?.Message);
        }
    }
}
