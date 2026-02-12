using Projects;

const string DatabaseProvider = nameof(DatabaseProvider);
const string PostgreDb = nameof(PostgreDb);
const string JwtOptionsSecretKey = "JwtOptions:SecretKey";
const string SecretKey = "quite-a-long-security-key-for-no-one-would-say-the-key-is-not-long-enough-not-to-be-too-short";

var builder = DistributedApplication.CreateBuilder(args);

var postgre = builder
    .AddPostgres("PostgreServer")
    .WithPgAdmin();

var db = postgre.AddDatabase(PostgreDb, "true_db");

var migrator = builder
    .AddProject<True_Data_Impl_Migrator_Svc>("MigratorJob")
    .WithEnvironment(DatabaseProvider, PostgreDb)
    .WithReference(db)
    .WaitFor(db);

var integrationCbr = builder
    .AddProject<True_Integration_Cbr>("IntegrationCbr")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithEnvironment(DatabaseProvider, PostgreDb)
    .WithReference(db)
    .WaitForCompletion(migrator);

var userApi = builder
    .AddProject<True_Users_Api>("UserApi")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithEnvironment(JwtOptionsSecretKey, SecretKey)
    .WithEnvironment(DatabaseProvider, PostgreDb)
    .WithReference(db)
    .WaitForCompletion(migrator);

var financeApi = builder
    .AddProject<True_Finance_Api>("FinanceApi")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithEnvironment(JwtOptionsSecretKey, SecretKey)
    .WithEnvironment(DatabaseProvider, PostgreDb)
    .WithReference(db)
    .WaitForCompletion(migrator);

var gatewayApi = builder
    .AddProject<True_Gateway_Api>("GatewayApi")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithEnvironment(JwtOptionsSecretKey, SecretKey)
    .WithEnvironment(
        "UsersApiClient:BaseAddress",
        userApi.Resource.GetEndpoint("https")
    )
    .WithEnvironment(
        "FinanceApiClient:BaseAddress",
        financeApi.Resource.GetEndpoint("https")
    )
    .WaitForCompletion(migrator);

builder.Build().Run();
