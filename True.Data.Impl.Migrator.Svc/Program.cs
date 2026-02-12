using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using True.Common.ServiceDefaults;
using True.Data.Impl.Migrator.Background;
using True.Data.Impl.Postgre;
using True.Data.Impl.Seed;

Log.Logger = LogExtensions.CreateBootstrapLogger();

try
{
    Log.Information("Pursuing to apply the {dbContext} migrations", nameof(TrueDbContextPostgre));

    var builder = Host.CreateApplicationBuilder();
    var appsettings = builder.Configuration;

    builder.RegisterServiceDefaults();
    builder.RegisterRuntimeLogger();

    builder.RegisterDataImplementationSeed();
    builder.RegisterDataImplementationPostgre();
    
    builder.Services.AddHostedService<Worker>();

    var app = builder.Build();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "The migration endeavor have resulted in a fiasco");
}
finally
{
    Log.Information("Consummated the {dbContext} migrations", nameof(TrueDbContextPostgre));
    Log.CloseAndFlush();
}