using Asset.Api.Swagger;
using Microsoft.OpenApi.Models;

namespace Asset.Api.ServiceInjection;

public static class SwaggerExtension
{

    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
        .AddSwaggerGen(c =>
        {
            // Define Bearer token authentication.
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });


            // Apply the requirement to all endpoints by adding it to the global security requirements.
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new string[] {}
                    }
                });
        })
        .ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }

}
