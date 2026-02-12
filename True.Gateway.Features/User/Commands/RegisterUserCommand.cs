using FluentResults;
using MediatR;

namespace True.User.Features.Commands
{
    public record RegisterUserCommand(string Name, string Password) : IRequest<Result>;
}
