using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using True.Gateway.Infrastructure.Finance.Abstractions;

namespace True.Gateway.Infrastructure.Finance.Implementations
{
    public class FinanceApiClient(HttpClient HttpClient)
        : IFinanceApiClient
    {
        public async Task<List<CurrencyDto>> GetCurrencies(FavoritesDto request, CancellationToken cancellationToken)
        {
            var query = QueryHelpers.AddQueryString("/api/currencies", new Dictionary<string, string?>()
            {
                ["favorites"] = string.Join(',', request.Ids.Select(f => f.Trim()))
            });

            var message = new HttpRequestMessage(HttpMethod.Get, query);

            using var response = await HttpClient.SendAsync(message, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var currencies = await response.Content.ReadFromJsonAsync<List<CurrencyDto>>(cancellationToken)
                    ?? new List<CurrencyDto>(0);

                return currencies;
            }

            return new List<CurrencyDto>(0);
        }
    }
}
