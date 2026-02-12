using FluentResults;
using MediatR;
using True.Users.Infrastructure.Abstractions;

namespace True.Users.Features.Commands
{
    public record RegisterUserCommand(User User) : IRequest<Result>;
}
