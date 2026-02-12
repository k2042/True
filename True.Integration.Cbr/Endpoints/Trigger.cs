using Quartz;
using True.Integration.Cbr.Configurations;

namespace True.Integration.Cbr.Endpoints
{
    public static class Trigger
    {
        public static void MapEndpoint(IEndpointRouteBuilder route)
        {
            route
                .MapGet("/", Handler);
        }

        private static async Task<IResult> Handler(
            ISchedulerFactory schedulerFactory,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken)
        {
            try
            {
                var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
                await scheduler.TriggerJob(new JobKey(QuartzConfigurations.LoadCurrenciesJobName));
                return Results.Ok($"{QuartzConfigurations.LoadCurrenciesJobName} triggered succesfully");
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger(nameof(Trigger));
                logger.LogError(e, "Error triggering {job} job",  QuartzConfigurations.LoadCurrenciesJobName);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
