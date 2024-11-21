using Asset.Application.Behaviors;
using Asset.Application.Services.Auth.Auth;
using MediatR;

namespace Asset.Api.ServiceInjection;

public static class MediatRExtension
{

    public static IServiceCollection AddMediatRExtension(this IServiceCollection services)
    {
        // Mention any command here
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<GetTokenQueryHandler>();
            //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        return services;
    }

}
