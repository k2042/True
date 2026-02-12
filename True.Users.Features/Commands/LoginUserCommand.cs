using FluentResults;
using MediatR;

namespace True.Users.Features.Commands
{
    public record LoginUserCommand(string Name, string Password) : IRequest<Result>;
}
