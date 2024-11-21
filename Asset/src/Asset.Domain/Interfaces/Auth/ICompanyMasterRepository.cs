using Asset.Domain.Common;
using Asset.Domain.Entities.Auth;

namespace Asset.Domain.Interfaces.Auth;

public interface ICompanyMasterRepository
{
    Task<CompanyMaster?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> AddAsync(CompanyMaster entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(CompanyMaster entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(CompanyMaster entity, CancellationToken cancellationToken);
    Task<IEnumerable<CompanyMaster>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken);
}
