using FluentResults;
using MediatR;
using True.Gateway.Infrastructure.User.Abstractions;
using True.User.Features.Commands;

namespace True.Gateway.Features.User.Handlers
{
    public class LoginUserHandler(IUsersApiClient Client)
        : IRequestHandler<LoginUserCommand, Result>
    {
        public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await Client.LoginUser(
                new UserDto(request.Name, request.Password),
                cancellationToken
            );

            return result.Success
                ? Result.Ok()
                : Result.Fail(result.Error);
        }
    }
}
