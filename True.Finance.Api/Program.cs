using Serilog;
using True.Data.Impl.Postgre;
using True.Common.Auth;
using True.Finance.Features;
using True.Finance.Infrastructure;
using True.Common.ServiceDefaults;
using True.Finance.Api.Endpoints;

Console.OutputEncoding = System.Text.Encoding.UTF8;

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
    builder.RegisterTokenAuthentication();
    builder.RegisterDataImplementationPostgre();
    builder.RegisterFinanceFeatures();
    builder.RegisterFinanceInfrastructure();

    var app = builder.Build();

    app.UseExceptionHandler();

    // default endpoints and requests
    app.MapDefaultEndpoints();
    app.AddOpenApiDocumentAndClient();
    app.UseSerilogRequestLogging();

    // app endpoints
    var currencies = app
        .MapGroup("/api/currencies")
        .WithTags("Currency operations");

    GetCurrencies.MapEndpointTo(currencies);

    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

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