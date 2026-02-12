using System.Runtime.CompilerServices;

namespace True.Gateway.Infrastructure.User.Abstractions
{
    public record LoginUserResponse(bool Success, string? Error = null);
}
