using Asset.Domain.Common;
using Asset.Domain.Entities.BookInventory;

namespace Asset.Domain.Interfaces.BookInventory;

public interface IBookMasterRepository
{
    Task<BookMaster?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> AddAsync(BookMaster entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(BookMaster entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(BookMaster entity, CancellationToken cancellationToken);
    Task<IEnumerable<BookMaster>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<int> CountAsync(QueryParams queryParams, CancellationToken cancellationToken);
}
