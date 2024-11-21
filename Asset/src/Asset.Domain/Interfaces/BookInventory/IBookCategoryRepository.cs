using Asset.Domain.Common;
using Asset.Domain.Entities.BookInventory;

namespace Asset.Domain.Interfaces.BookInventory;

public interface IBookCategoryRepository
{
    Task<IEnumerable<dynamic>> GetAllAsDropdownAsync(CancellationToken cancellationToken);
    Task<BookCategory?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> AddAsync(BookCategory entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(BookCategory entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(BookCategory entity, CancellationToken cancellationToken);
    Task<IEnumerable<BookCategory>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken);
}
