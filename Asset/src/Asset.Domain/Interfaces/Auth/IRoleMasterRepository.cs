using Asset.Domain.Common;
using Asset.Domain.Entities.Auth.Identity;

namespace Asset.Domain.Interfaces.Auth;

public interface IRoleMasterRepository
{
    Task<RoleMaster?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserRolesAsync(UserMaster user, CancellationToken cancellationToken = default);


    Task<RoleMaster?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> AddAsync(RoleMaster entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(RoleMaster entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(RoleMaster entity, CancellationToken cancellationToken);
    Task<IEnumerable<RoleMaster>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken);
}
