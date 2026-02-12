using System.Net.Http.Json;
using System.Xml.Serialization;
using True.Integration.Cbr.Infrastructure.Currency.Dto;

namespace True.Integration.Cbr.Infrastructure.Currency.Client
{
    public class CurrenciesApiClient
    {
        private readonly HttpClient _httpClient;

        public CurrenciesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetCurrenciesResponse> GetCurrencies(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/scripts/XML_daily.asp");
            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var serializer = new XmlSerializer(typeof(ValCurs));
                var result = serializer.Deserialize(stream) as ValCurs;

                return new GetCurrenciesResponse(
                    Data: result
                );
            }

            return new GetCurrenciesResponse(
                Error: await response.Content.ReadAsStringAsync(cancellationToken)
            );

        }
    }
}
