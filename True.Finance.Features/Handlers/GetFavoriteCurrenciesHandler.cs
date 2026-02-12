using FluentResults;
using MediatR;
using True.Finance.Features.Queries;
using True.Finance.Infrastructure.Abstractions;

namespace True.Finance.Features.Handlers
{
    public class GetFavoriteCurrenciesHandler(
        ICurrencyRepository Repository
    ) : IRequestHandler<GetFavoriteCurrenciesQuery, Result<List<Currency>>>
    {
        public async Task<Result<List<Currency>>> Handle(
            GetFavoriteCurrenciesQuery request,
            CancellationToken cancellationToken
        )
        {
            var currencies = await Repository.GetCurrenciesList(request.Favorites, cancellationToken);

            return Result.Ok(currencies);
        }
    }
}
