using Asp.Versioning.ApiExplorer;
using Asset.Api.ServiceInjection;
using Serilog;

namespace Asset.Api;

public static class Startup
{
    public static IServiceCollection AddApiLayerExtensions(this IServiceCollection services)
    {
        services
            .AddApiVersioningExtension()
            .AddRateLimiterExtension()
            .AddCommonServicesExtension()
            .AddMiddlewareExtension()
            .AddCorsExtension()
            .AddMediatRExtension()
            .AddExceptionHandlerExtension()
            .AddSwaggerExtension()
            .AddJwtAuthenticationExtension();

        return services;
    }


    public static IApplicationBuilder UsePipilines(this IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app
             .UseSwagger()
             .UseSwaggerUI(o =>
             {
                 foreach (var description in provider.ApiVersionDescriptions)
                 {
                     o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                 }

                 o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
             });
        }
        else
        {
            app
             .UseSwagger()
             .UseSwaggerUI(o =>
             {
                 foreach (var description in provider.ApiVersionDescriptions)
                 {
                     o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                 }

                 o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
             });
        }

        // Apply rate limiting
        app.UseRateLimiter()

        // Serve static files
        .UseStaticFiles()

        // Apply exception handling
        .UseExceptionHandler()

        // Log requests using Serilog
        .UseSerilogRequestLogging()

        // Enable the CORS middleware with the defined policy
        .UseCors("DefaultCorePolicy")

        // Redirect HTTP to HTTPS
        .UseHttpsRedirection()

        // Enable endpoint routing
        .UseRouting()

        // Enable authentication
        .UseAuthentication()

        // Enable authorization
        .UseAuthorization()

        // Map controllers
        .UseEndpoints(endpoints => endpoints
            .MapControllers()
        );

        return app;
    }
}
