using Asset.Domain.Common;
using Asset.Domain.Entities.BookInventory;
using Asset.Domain.Enums;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;
using System.Linq.Dynamic.Core;

namespace Asset.Infrastructure.Repositories.BookInventory;

internal class BookTransactionRepository(IUnitOfWork _unitOfWork) : IBookTransactionRepository
{
    public async Task<bool> AddAsync(BookTransaction entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Add(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<bool> UpdateAsync(BookTransaction entity, CancellationToken cancellationToken)
    {
        _unitOfWork.Repository().Update(entity);
        var effectedRows = await _unitOfWork.CommitAsync(cancellationToken);

        return effectedRows > 0;
    }
    public async Task<int> CountAllAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookTransaction>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm),
            cancellationToken);
    }
    public async Task<int> BorrowedCountAsync(long bookId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookTransaction>(
           x => x.BookId == bookId && x.ReturnedDate == null && x.TransactionType == TransactionTypes.Borrowed,
           cancellationToken);
    }



    public async Task<int> CountUserTransactionsAsync(long userId, QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookTransaction>(
            x => x.UserId == userId && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm)),
            cancellationToken);
    }
    public async Task<int> CountBookTransactionsAsync(long bookId, QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookTransaction>(
            x => x.BookId == bookId && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm)),
            cancellationToken);
    }
    public async Task<int> CountUserBookTransactionsAsync(long userId, long bookId, QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().CountAsync<BookTransaction>(
            x => x.UserId == userId && x.BookId == bookId && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm)),
            cancellationToken);
    }




    public async Task<IEnumerable<BookTransaction>> GetBookTransactionsAsync(long bookId, QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().FindAsync<BookTransaction>(
            x => x.BookId == bookId && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm)),
            o => o.OrderByDescending(x => x.Id),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken,
            nameof(BookTransaction.GetUser),
            nameof(BookTransaction.GetBook));
    }
    public async Task<IEnumerable<BookTransaction>> GetUserTransactionsAsync(long userId, QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().FindAsync<BookTransaction>(
            x => x.UserId == userId && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm)),
            o => o.OrderByDescending(x => x.Id),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken,
            nameof(BookTransaction.GetUser),
            nameof(BookTransaction.GetBook));
    }
    public async Task<IEnumerable<BookTransaction>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var orderByExpression = queryParams.Ascending ? queryParams.SortColumn : $"{queryParams.SortColumn} DESC";

        return await _unitOfWork.Repository().FindAsync<BookTransaction>(
            x => string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm),
            o => o.OrderBy(orderByExpression),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken,
            nameof(BookTransaction.GetUser),
            nameof(BookTransaction.GetBook));
    }
    public async Task<IEnumerable<BookTransaction>> GetUserBookTransactionsAsync(long userId, long bookId, QueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository().FindAsync<BookTransaction>(
            x => x.UserId == userId && x.BookId == bookId && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || x.GetBook.Title.Contains(queryParams.SearchTerm)),
            o => o.OrderByDescending(x => x.Id),
            queryParams.PageNumber,
            queryParams.PageSize,
            cancellationToken,
            nameof(BookTransaction.GetUser),
            nameof(BookTransaction.GetBook));
    }




    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
    }
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
    }
}
