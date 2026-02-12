using Quartz;
using True.Integration.Cbr.Jobs;

namespace True.Integration.Cbr.Configurations
{
    public static class QuartzConfigurations
    {
        public const string LoadCurrenciesJobName = nameof(LoadCurrenciesJob);
        public const string LoadCurrenciesJobTriggerName = LoadCurrenciesJobName + "Trigger";

        public static IHostApplicationBuilder ConfigureQuartz(this IHostApplicationBuilder builder)
        {
            var appsettings = builder.Configuration;
            var services = builder.Services;

            services.AddQuartz(static quartz =>
            {
                quartz.AddJob<LoadCurrenciesJob>(static options => 
                {
                    options
                        .WithIdentity(LoadCurrenciesJobName)
                        .StoreDurably();     
                });

                quartz.AddTrigger(static options =>
                {
                    options
                        .ForJob(LoadCurrenciesJobName)
                        .WithIdentity(LoadCurrenciesJobTriggerName)
                        .WithCronSchedule(CronScheduleBuilder.DailyAtHourAndMinute(7, 0));
                });
            });

            services.AddQuartzHostedService(static config =>
            {
                config.AwaitApplicationStarted = true;
                config.WaitForJobsToComplete = true;
            });

            return builder;
        }
    }
}
