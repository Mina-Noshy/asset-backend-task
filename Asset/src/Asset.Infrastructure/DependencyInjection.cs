using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Interfaces.Auth;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;
using Asset.Domain.Utilities;
using Asset.Infrastructure.Persistence;
using Asset.Infrastructure.Repositories;
using Asset.Infrastructure.Repositories.Auth;
using Asset.Infrastructure.Repositories.BookInventory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Asset.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayerExtensions(this IServiceCollection services)
    {
        services
            .AddContexts()
            .AddIdentity()
            .AddRepositories()
            .AddDatabaseStandard();

        return services;
    }

    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            string connectionString = ConfigurationHelper.GetConnectionString();

            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly("Asset.Api");
                sqlServerOptions.CommandTimeout(600);
            });
            options.EnableSensitiveDataLogging();
        }, ServiceLifetime.Scoped);

        services.AddScoped<IMainDbContext, MainDbContext>();

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<UserMaster, RoleMaster>(
            options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddDatabaseStandard(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<IDatabaseRouter, DatabaseRouter>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();
        services.AddScoped<ICompanyMasterRepository, CompanyMasterRepository>();

        // BookInventory
        services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
        services.AddScoped<IBookMasterRepository, BookMasterRepository>();
        services.AddScoped<IBookTransactionRepository, BookTransactionRepository>();

        return services;
    }
}
