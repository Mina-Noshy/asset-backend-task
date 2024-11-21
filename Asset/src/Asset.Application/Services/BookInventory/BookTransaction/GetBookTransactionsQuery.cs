using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookTransaction;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.BookInventory;

namespace Asset.Application.Services.BookInventory.BookTransaction;

public record GetBookTransactionsQuery
    (long bookId, QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetBookTransactionsQueryHandler(IBookTransactionRepository _repository) : IQueryHandler<GetBookTransactionsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetBookTransactionsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.CountBookTransactionsAsync(request.bookId, request.queryParams, cancellationToken);

        var entityList = await _repository.GetBookTransactionsAsync(request.bookId, request.queryParams, cancellationToken);

        var entityListDto = entityList.Select(x => new BookTransactionDto()
        {
            Id = x.Id,
            BookId = x.BookId,
            UserId = x.UserId,
            BookTitle = x.GetBook.Title,
            UserName = x.GetUser.UserName ?? throw new NullReferenceException("UserName property is null, something in App flow is wrong"),
            DueDate = x.DueDate,
            IsOverdue = x.IsOverdue,
            ReturnedDate = x.ReturnedDate,
            TransactionDate = x.TransactionDate,
            TransactionType = x.TransactionType.ToString()
        });
        var pagedResponse = new PagedResponse<BookTransactionDto>(entityListDto, request.queryParams.PageNumber, request.queryParams.PageSize, totalCount);

        return new ApiResponse(ResultType.Success, pagedResponse);
    }
}
