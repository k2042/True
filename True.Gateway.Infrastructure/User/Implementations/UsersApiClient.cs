using System.Net.Http.Json;
using True.Gateway.Infrastructure.User.Abstractions;

namespace True.Gateway.Infrastructure.User.Implementations
{
    public class UsersApiClient(HttpClient HttpClient)
        : IUsersApiClient
    {
        public async Task<RegisterUserResponse> RegisterUser(UserDto dto, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/users/register")
            {
                Content = JsonContent.Create(dto)
            };

            using var response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return new RegisterUserResponse(true);
            }

            return new RegisterUserResponse(
                false,
                await response.Content.ReadAsStringAsync()
            );
        }

        public async Task<LoginUserResponse> LoginUser(UserDto dto, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/users/login")
            {
                Content = JsonContent.Create(dto)
            };

            using var response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return new LoginUserResponse(true);
            }

            return new LoginUserResponse(
                false,
                await response.Content.ReadAsStringAsync()
            );
        }
    }
}
