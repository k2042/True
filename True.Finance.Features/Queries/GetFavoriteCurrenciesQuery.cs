using FluentResults;
using MediatR;
using True.Finance.Infrastructure.Abstractions;

namespace True.Finance.Features.Queries
{
    public record GetFavoriteCurrenciesQuery(IEnumerable<string> Favorites) : IRequest<Result<List<Currency>>>;
}
