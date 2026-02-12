using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using True.Data.Model.Dbo;
using True.Data.Model;
using True.Integration.Cbr.Features.Currencies.Commands;
using True.Integration.Cbr.Infrastructure.Currency.Client;

namespace True.Integration.Cbr.Features.Currencies.Handlers
{
    public class LoadCurrenciesHandler : IRequestHandler<LoadCurrenciesCommand, LoadCurrenciesCommandResult>
    {
        private readonly CurrenciesApiClient _apiClient;
        private readonly ITrueDbContext _dbContext;
        private readonly ILogger<LoadCurrenciesHandler> _logger;

        public LoadCurrenciesHandler(
            CurrenciesApiClient apiClient,
            ITrueDbContext dbContext,
            ILogger<LoadCurrenciesHandler> logger
        )
        {
            _apiClient = apiClient;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<LoadCurrenciesCommandResult> Handle(LoadCurrenciesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var apiResult = await _apiClient.GetCurrencies(cancellationToken);

                if (apiResult.Data == null)
                {
                    return new LoadCurrenciesCommandResult(
                        Rows: 0,
                        Error: apiResult.Error
                    );
                }

                var currencies = apiResult.Data.Valute
                    .Select(v => new CurrencyDbo()
                    {
                        Id = v.CharCode,
                        Name = v.Name,
                        Rate = decimal.Parse(v.VunitRate, NumberStyles.Currency | NumberStyles.AllowExponent)
                    });

                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await _dbContext.Currencies.ExecuteDeleteAsync(cancellationToken);
                _dbContext.Currencies.AddRange(currencies);
                var rows = await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new LoadCurrenciesCommandResult(
                    Rows: rows
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading CBR currency rates");
                return new LoadCurrenciesCommandResult(
                    Rows: 0,
                    Error: e.Message
                );
            }
        }
    }
}
