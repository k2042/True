using System.Runtime.CompilerServices;

namespace True.Gateway.Infrastructure.User.Abstractions
{
    public record RegisterUserResponse(bool Success, string? Error = null);
}
