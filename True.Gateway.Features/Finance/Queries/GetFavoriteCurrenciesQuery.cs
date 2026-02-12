using FluentResults;
using MediatR;
using True.Gateway.Infrastructure.Finance.Abstractions;

namespace True.Gateway.Features.Finance.Queries
{
    public record GetFavoriteCurrenciesQuery(IEnumerable<string> Favorites) : IRequest<Result<List<CurrencyDto>>>;
}
