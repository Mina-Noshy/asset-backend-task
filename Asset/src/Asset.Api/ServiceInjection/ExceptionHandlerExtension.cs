using Asset.Api.ExceptionHandlers;
using Asset.Application.Services.Auth.Auth;
using FluentValidation;

namespace Asset.Api.ServiceInjection;

public static class ExceptionHandlerExtension
{
    public static IServiceCollection AddExceptionHandlerExtension(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddValidatorsFromAssemblyContaining<GetTokenQueryHandler>();

        return services;
    }
}
