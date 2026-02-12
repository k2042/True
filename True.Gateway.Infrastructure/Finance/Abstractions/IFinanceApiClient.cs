namespace True.Gateway.Infrastructure.Finance.Abstractions
{
    public interface IFinanceApiClient
    {
        Task<List<CurrencyDto>> GetCurrencies(FavoritesDto dto, CancellationToken cancellationToken);
    }
}
