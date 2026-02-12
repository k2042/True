using FluentResults;
using MediatR;
using True.Gateway.Infrastructure.User.Abstractions;
using True.User.Features.Commands;

namespace True.Gateway.Features.User.Handlers
{
    public class RegisterUserHandler(IUsersApiClient Client)
        : IRequestHandler<RegisterUserCommand, Result>
    {
        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await Client.RegisterUser(
                new UserDto(request.Name, request.Password),
                cancellationToken
            );

            return result.Success
                ? Result.Ok()
                : Result.Fail(result.Error);
        }
    }
}
