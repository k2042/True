using FluentResults;
using MediatR;
using True.Gateway.Features.Finance.Queries;
using True.Gateway.Infrastructure.Finance.Abstractions;

namespace True.Gateway.Features.Finance.Handlers
{
    public class GetFavoriteCurrenciesHandler(IFinanceApiClient Client)
        : IRequestHandler<GetFavoriteCurrenciesQuery, Result<List<CurrencyDto>>>
    {
        public async Task<Result<List<CurrencyDto>>> Handle(
            GetFavoriteCurrenciesQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = await Client.GetCurrencies(new FavoritesDto(request.Favorites), cancellationToken);

            return result;
        }
    }
}
