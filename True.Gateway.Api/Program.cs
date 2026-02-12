using Serilog;
using True.Common.Auth;
using True.Common.ServiceDefaults;
using True.Gateway.Api.Configurations;
using True.Gateway.Api.Endpoints;
using True.Gateway.Features;
using True.Gateway.Infrastructure;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Log.Logger = LogExtensions.CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // logging, observability, api discovery
    builder.RegisterServiceDefaults();
    builder.RegisterOpenApiServices();
    builder.RegisterRuntimeLogger();
    builder.RegisterCommonExceptionHandler();

    // business-logic
    builder.RegisterTokenAuthentication();
    builder.RegisterCookieAuthentication();
    builder.RegisterGatewayInfrastructure();
    builder.RegisterGatewayHandlers();

    var app = builder.Build();

    app.UseExceptionHandler();

    // default endpoints and requests
    app.MapDefaultEndpoints();
    app.AddOpenApiDocumentAndClient();
    app.UseSerilogRequestLogging();

    // app endpoints
    var users = app
        .MapGroup("/api/users")
        .WithTags("User operations")
        .DisableAntiforgery();

    RegisterUser.MapEndpoint(users);
    LoginUser.MapEndpoint(users);
    LogoutUser.MapEndpoint(users);

    var token = app
        .MapGroup("/api/token")
        .WithTags("Token operations")
        .DisableAntiforgery();

    RefreshToken.MapEndpoint(token);
    
    var currencies = app
        .MapGroup("/api/currencies")
        .WithTags("Currency operations");

    GetCurrencies.MapEndpoint(currencies);

    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

    // http-pipeline
    app.UseHttpsRedirection();
    app.UseHeaderPropagation();

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