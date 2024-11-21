using Asset.Domain.Common;
using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace Asset.Infrastructure.Repositories.Auth;

internal class RoleMasterRepository(RoleManager<RoleMaster> _roleManager, UserManager<UserMaster> _userManager) : IRoleMasterRepository
{
    public async Task<RoleMaster?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _roleManager.FindByNameAsync(name);
    }
    public async Task<IEnumerable<string>> GetUserRolesAsync(UserMaster user, CancellationToken cancellationToken = default)
    {
        return await _userManager.GetRolesAsync(user);
    }


    public async Task<RoleMaster?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return
            await _roleManager.FindByIdAsync(id.ToString());
    }
    public async Task<bool> AddAsync(RoleMaster entity, CancellationToken cancellationToken)
    {
        var role = await _roleManager.CreateAsync(entity);

        return role.Succeeded;
    }
    public async Task<bool> UpdateAsync(RoleMaster entity, CancellationToken cancellationToken)
    {
        var role = await _roleManager.UpdateAsync(entity);

        return role.Succeeded;
    }
    public async Task<bool> DeleteAsync(RoleMaster entity, CancellationToken cancellationToken)
    {
        var role = await _roleManager.DeleteAsync(entity);

        return role.Succeeded;
    }
    public async Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _roleManager.Roles.CountAsync(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(queryParams.SearchTerm)),
            cancellationToken);
    }
    public async Task<IEnumerable<RoleMaster>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var orderByExpression = queryParams.Ascending ? queryParams.SortColumn : $"{queryParams.SortColumn} DESC";

        return await _roleManager.Roles
            .Where(x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || (x.Name + x.NormalizedName).Contains(queryParams.SearchTerm))
            .OrderBy(orderByExpression)
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync(cancellationToken);
    }
}
