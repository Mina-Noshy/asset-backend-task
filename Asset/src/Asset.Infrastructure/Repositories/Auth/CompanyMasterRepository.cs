using Asset.Domain.Common;
using Asset.Domain.Entities.Auth;
using Asset.Domain.Interfaces.Auth;
using Asset.Domain.Interfaces.Common;
using System.Linq.Dynamic.Core;

namespace Asset.Infrastructure.Repositories.Auth;

internal class CompanyMasterRepository(IUnitOfWork _unitOfWork) : ICompanyMasterRepository
{
    public async Task<CompanyMaster?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return
            await _unitOfWork.Repository().GetByIdAsync<CompanyMaster>(id, cancellationToken);
    }
    public async Task<bool> AddAsync(CompanyMaster entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Add(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> UpdateAsync(CompanyMaster entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Update(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> DeleteAsync(CompanyMaster entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Remove(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<CompanyMaster>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.CompanyName.Contains(queryParams.SearchTerm),
            cancellationToken);
    }
    public async Task<IEnumerable<CompanyMaster>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var orderByExpression = queryParams.Ascending ? queryParams.SortColumn : $"{queryParams.SortColumn} DESC";

        return await _unitOfWork.Repository().FindAsync<CompanyMaster>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.CompanyName.Contains(queryParams.SearchTerm),
            o => o.OrderBy(orderByExpression),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken);
    }
}
