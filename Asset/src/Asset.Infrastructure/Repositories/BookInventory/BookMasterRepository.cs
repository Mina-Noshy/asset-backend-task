using Asset.Domain.Common;
using Asset.Domain.Entities.BookInventory;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;
using System.Linq.Dynamic.Core;

namespace Asset.Infrastructure.Repositories.BookInventory;

internal class BookMasterRepository(IUnitOfWork _unitOfWork) : IBookMasterRepository
{
    public async Task<BookMaster?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return
            await _unitOfWork.Repository().GetByIdAsync<BookMaster>(id, cancellationToken, nameof(BookMaster.GetCategory));
    }
    public async Task<bool> AddAsync(BookMaster entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Add(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> UpdateAsync(BookMaster entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Update(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> DeleteAsync(BookMaster entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Remove(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookMaster>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.Title.Contains(queryParams.SearchTerm),
            cancellationToken);
    }
    public async Task<IEnumerable<BookMaster>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var orderByExpression = queryParams.Ascending ? queryParams.SortColumn : $"{queryParams.SortColumn} DESC";

        return await _unitOfWork.Repository().FindAsync<BookMaster>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.Title.Contains(queryParams.SearchTerm),
            o => o.OrderBy(orderByExpression),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken,
            nameof(BookMaster.GetCategory));
    }
}
