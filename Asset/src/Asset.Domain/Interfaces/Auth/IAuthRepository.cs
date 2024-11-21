using Asset.Domain.Entities.Auth.Identity;
using Microsoft.AspNetCore.Identity;

namespace Asset.Domain.Interfaces.Auth;

public interface IAuthRepository
{
    Task<IdentityResult> AddUserRoleAsync(UserMaster user, string role, CancellationToken cancellationToken = default);
    Task<IdentityResult> RemoveUserRoleAsync(UserMaster user, string role, CancellationToken cancellationToken = default);


    Task<bool> CheckPasswordAsync(UserMaster user, string password, CancellationToken cancellationToken = default);
    Task UpdateUser(UserMaster user, CancellationToken cancellationToken = default);
}
