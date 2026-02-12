using Microsoft.EntityFrameworkCore;
using True.Data.Model;
using True.Finance.Infrastructure.Abstractions;

namespace True.Finance.Infrastructure.Implementations
{
    public class CurrencyRepository(ITrueDbContext DbContext)
        : ICurrencyRepository
    {
        public async Task<List<Currency>> GetCurrenciesList(IEnumerable<string> idList, CancellationToken cancellationToken)
        {
            var ids = idList.Select(f => f.ToUpper());

            var currencies = await DbContext.Currencies
                .AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .Select(c => new Currency(c.Id, c.Name, c.Rate))
                .ToListAsync(cancellationToken);

            return currencies;
        }
    }
}
