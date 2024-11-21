using Asp.Versioning.ApiExplorer;
using Asset.Api;
using Asset.Api.Configurations;
using Asset.Api.Loggers;
using Asset.Domain;
using Asset.Domain.Utilities;
using Asset.Infrastructure;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations();
    ConfigurationHelper.Initialize(builder.Configuration, builder.Environment);

    SerilogConfig.EnsureInitialized();
    Log.Information("Starting Asset web host");

    builder.Host.UseSerilog();

    // Register All Layers
    builder.Services.AddApiLayerExtensions();
    builder.Services.AddDomainLayerExtensions();
    builder.Services.AddInfrastructureLayerExtensions();

    var app = builder.Build();

    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UsePipilines(app.Environment, provider);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception :: Asset Api server start-up failed");
}
finally
{
    Log.Information("Asset Api server shutting down...");
    Log.CloseAndFlush();
}