namespace Asset.Api.ServiceInjection;

public static class CommonServicesExtension
{
    public static IServiceCollection AddCommonServicesExtension(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers().AddNewtonsoftJson();
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        return services;
    }
}
