using Asset.Domain.Constants;
using Asset.Domain.Entities.Auth.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asset.Infrastructure.Configurations;


internal static class DbContextUtilities
{
    internal static string HashPassword(UserMaster user, string password)
    {
        var passwordHasher = new PasswordHasher<UserMaster>();
        return passwordHasher.HashPassword(user, password);
    }
}

internal sealed class RoleMasterConfigurations : IEntityTypeConfiguration<RoleMaster>
{
    public void Configure(EntityTypeBuilder<RoleMaster> entity)
    {
        var items = new List<RoleMaster>()
        {
            new RoleMaster{Id = 1, Name = AuthenticationRoles.ADMIN, NormalizedName = AuthenticationRoles.ADMIN.ToUpper()},
            new RoleMaster{Id = 2, Name = AuthenticationRoles.USER, NormalizedName = AuthenticationRoles.USER.ToUpper()},
        };

        entity.HasData(items);
    }
}

internal sealed class UserMasterConfigurations : IEntityTypeConfiguration<UserMaster>
{
    public void Configure(EntityTypeBuilder<UserMaster> entity)
    {
        // Enforce unique contact numbers
        entity.HasIndex(u => u.PhoneNumber).IsUnique();


        var user = new UserMaster
        {
            Id = 1,
            FirstName = "Asset",
            LastName = "Technology",

            UserName = "asset",
            NormalizedUserName = "asset".ToUpper(),

            Email = "info@asset.com",
            NormalizedEmail = "info@asset.com".ToUpper(),
            EmailConfirmed = true,

            PhoneNumber = "0020111111111",
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),

            IsActive = true,
            IsBlocked = false
        };

        user.PasswordHash = DbContextUtilities.HashPassword(user, "asset123");

        entity.HasData(user);
    }
}

internal sealed class UserRoleMasterConfigurations : IEntityTypeConfiguration<UserRoleMaster>
{
    public void Configure(EntityTypeBuilder<UserRoleMaster> entity)
    {
        var userRole = new UserRoleMaster { UserId = 1, RoleId = 1 };

        entity.HasData(userRole);
    }
}
