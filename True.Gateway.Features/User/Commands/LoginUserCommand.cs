using FluentResults;
using MediatR;

namespace True.User.Features.Commands
{
    public record LoginUserCommand(string Name, string Password) : IRequest<Result>;
}
