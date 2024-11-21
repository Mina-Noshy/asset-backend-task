using Asset.Domain.Common;
using Asset.Domain.Entities.BookInventory;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;
using System.Linq.Dynamic.Core;

namespace Asset.Infrastructure.Repositories.BookInventory;

internal class BookCategoryRepository(IUnitOfWork _unitOfWork) : IBookCategoryRepository
{
    public async Task<IEnumerable<dynamic>> GetAllAsDropdownAsync(CancellationToken cancellationToken)
    {
        return
            await _unitOfWork.Repository().GetAllDynamicAsync<BookCategory>(x =>
                true,
                x => new { id = x.Id, name = x.Name },
                x => x.OrderBy(o => o.Name),
                cancellationToken);
    }
    public async Task<BookCategory?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return
            await _unitOfWork.Repository().GetByIdAsync<BookCategory>(id, cancellationToken);
    }
    public async Task<bool> AddAsync(BookCategory entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Add(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> UpdateAsync(BookCategory entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Update(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> DeleteAsync(BookCategory entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Remove(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookCategory>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.Name.Contains(queryParams.SearchTerm),
            cancellationToken);
    }
    public async Task<IEnumerable<BookCategory>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var orderByExpression = queryParams.Ascending ? queryParams.SortColumn : $"{queryParams.SortColumn} DESC";

        return await _unitOfWork.Repository().FindAsync<BookCategory>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.Name.Contains(queryParams.SearchTerm),
            o => o.OrderBy(orderByExpression),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken);
    }

}
