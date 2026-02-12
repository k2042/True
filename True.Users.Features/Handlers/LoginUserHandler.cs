using FluentResults;
using MediatR;
using True.Users.Features.Commands;
using True.Users.Infrastructure.Abstractions;

namespace True.Users.Features.Handlers
{
    public class LoginUserHandler(IUserRepository Repository)
        : IRequestHandler<LoginUserCommand, Result>
    {
        public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await Repository.FindUser(request.Name, cancellationToken);

            if (user == null || !string.Equals(user.Password, request.Password))
            {
                return Result.Fail("Wrong username or password");
            }

            return Result.Ok();
        }
    }
}
