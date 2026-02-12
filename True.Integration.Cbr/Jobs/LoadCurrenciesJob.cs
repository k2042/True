using MediatR;
using Quartz;
using True.Integration.Cbr.Features.Currencies.Commands;

namespace True.Integration.Cbr.Jobs
{
    public class LoadCurrenciesJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<LoadCurrenciesJob> _logger;

        public LoadCurrenciesJob(
            IServiceProvider provider,
            ILogger<LoadCurrenciesJob> logger
        )
        {
            _provider = provider;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var mediator = _provider.GetRequiredService<IMediator>();

                var result = await mediator.Send(new LoadCurrenciesCommand());

                if (result.Error != null)
                {
                    _logger.LogError("Error loading CBR currency rates: {error}", result.Error);
                }
                else
                {
                    _logger.LogInformation("CBR currency rates loaded: {rows} rows", result.Rows);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading CBR currency rates");
            }
        }
    }
}
