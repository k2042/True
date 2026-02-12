using Serilog;
using System.Text;
using True.Common.ServiceDefaults;
using True.Data.Impl.Postgre;
using True.Gateway.Features;
using True.Gateway.Infrastructure;
using True.Integration.Cbr.Configurations;
using True.Integration.Cbr.Endpoints;

Console.OutputEncoding = Encoding.UTF8;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Log.Logger = LogExtensions.CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // logging, observability, api discover.
    builder.RegisterServiceDefaults();
    builder.RegisterOpenApiServices();
    builder.RegisterRuntimeLogger();
    builder.RegisterCommonExceptionHandler();

    // business-logic
    builder.RegisterDataImplementationPostgre();
    builder.RegisterCbrClients();
    builder.RegisterIntegrationCbrHandlers();
    builder.ConfigureQuartz();

    var app = builder.Build();

    app.UseExceptionHandler();

    // default endpoints and requests
    app.MapDefaultEndpoints();
    app.AddOpenApiDocumentAndClient();
    app.UseSerilogRequestLogging();

    // app endpoints
    var trigger = app
        .MapGroup("/api/trigger")
        .WithTags("Trigger");

    Trigger.MapEndpoint(trigger);

    app.Map("/", () => Results.Redirect("/scalar"));

    app.UseHttpsRedirection();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Service startup ended up in a fiasco");
}
finally
{
    Log.CloseAndFlush();
}