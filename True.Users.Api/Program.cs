using Serilog;
using True.Common.Auth;
using True.Common.ServiceDefaults;
using True.Data.Impl.Postgre;
using True.Users.Api.Endpoints;
using True.Users.Features;
using True.Users.Infrastructure;

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
    builder.RegisterUsersFeatures();
    builder.RegisterUsersInfrastructure();

    var app = builder.Build();

    app.UseExceptionHandler();

    // default endpoints and requests
    app.MapDefaultEndpoints();
    app.AddOpenApiDocumentAndClient();
    app.UseSerilogRequestLogging();

    // app endpoints
    var users = app
        .MapGroup("api/users")
        .WithTags("Token operations");
    
    RegisterUser.MapEndpointTo(users);
    LoginUser.MapEndpointTo(users);

    app.Map("/", () => Results.Redirect("/scalar"));

    // http-pipeline
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