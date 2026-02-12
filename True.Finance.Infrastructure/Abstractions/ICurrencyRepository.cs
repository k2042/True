
namespace True.Finance.Infrastructure.Abstractions
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetCurrenciesList(IEnumerable<string> idList, CancellationToken cancellationToken);
    }
}
