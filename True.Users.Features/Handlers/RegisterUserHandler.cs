using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using True.Data.Model;
using True.Data.Model.Dbo;
using True.Users.Features.Commands;
using True.Users.Infrastructure.Abstractions;

namespace True.Users.Features.Handlers
{
    public class RegisterUserHandler(IUserRepository Repository) : IRequestHandler<RegisterUserCommand, Result>
    {
        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await Repository.FindUser(request.User.Name, cancellationToken);

            if (existingUser != null)
            {
                return Result.Fail($"User \"{request.User.Name}\" already exists");
            }

            await Repository.CreateUser(request.User, cancellationToken);

            return Result.Ok();
        }
    }
}

