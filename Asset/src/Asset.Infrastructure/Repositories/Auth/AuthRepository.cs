using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace Asset.Infrastructure.Repositories.Auth;

internal class AuthRepository(UserManager<UserMaster> _userManager) : IAuthRepository
{
    public async Task<IdentityResult> AddUserRoleAsync(UserMaster user, string role, CancellationToken cancellationToken = default)
        => await _userManager.AddToRoleAsync(user, role);

    public async Task<IdentityResult> RemoveUserRoleAsync(UserMaster user, string role, CancellationToken cancellationToken = default)
        => await _userManager.RemoveFromRoleAsync(user, role);

    public async Task<bool> CheckPasswordAsync(UserMaster user, string password, CancellationToken cancellationToken = default)
        => await _userManager.CheckPasswordAsync(user, password);


    public async Task UpdateUser(UserMaster user, CancellationToken cancellationToken = default)
    {
        await _userManager.UpdateAsync(user);
    }

}
