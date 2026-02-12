namespace True.Gateway.Infrastructure.User.Abstractions
{
    public interface IUsersApiClient
    {
        Task<RegisterUserResponse> RegisterUser(UserDto dto, CancellationToken cancellationToken);
        Task<LoginUserResponse> LoginUser(UserDto dto, CancellationToken cancellationToken);
    }
}
