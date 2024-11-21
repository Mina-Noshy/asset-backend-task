using Asset.Domain.Interfaces.Common;
using Asset.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Asset.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainLayerExtensions(this IServiceCollection services)
        {
            services
                .AddServices();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IPathFinderService, PathFinderService>();

            return services;
        }
    }
}
