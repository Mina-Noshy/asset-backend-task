using Asset.Domain.Common;
using Asset.Domain.Entities.BookInventory;

namespace Asset.Domain.Interfaces.BookInventory;

public interface IBookTransactionRepository
{
    Task<bool> AddAsync(BookTransaction entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(BookTransaction entity, CancellationToken cancellationToken);
    Task<int> BorrowedCountAsync(long bookId, CancellationToken cancellationToken);
    Task<int> CountAllAsync(QueryParams queryParams, CancellationToken cancellationToken);


    Task<int> CountUserTransactionsAsync(long userId, QueryParams queryParams, CancellationToken cancellationToken);
    Task<int> CountBookTransactionsAsync(long bookId, QueryParams queryParams, CancellationToken cancellationToken);
    Task<int> CountUserBookTransactionsAsync(long userId, long bookId, QueryParams queryParams, CancellationToken cancellationToken);


    Task<IEnumerable<BookTransaction>> GetUserTransactionsAsync(long userId, QueryParams queryParams, CancellationToken cancellationToken);
    Task<IEnumerable<BookTransaction>> GetBookTransactionsAsync(long bookId, QueryParams queryParams, CancellationToken cancellationToken);
    Task<IEnumerable<BookTransaction>> GetUserBookTransactionsAsync(long userId, long bookId, QueryParams queryParams, CancellationToken cancellationToken);
    Task<IEnumerable<BookTransaction>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken);


    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
