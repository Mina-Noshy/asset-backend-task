using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookTransaction;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;

namespace Asset.Application.Services.BookInventory.BookTransaction;

public record GetUserTransactionsQuery
    (QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetUserTransactionsQueryHandler(IBookTransactionRepository _repository, ICurrentUser _currentUser) : IQueryHandler<GetUserTransactionsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetUserTransactionsQuery request, CancellationToken cancellationToken)
    {
        long.TryParse(_currentUser.UserId, out long userId);

        var totalCount = await _repository.CountUserTransactionsAsync(userId, request.queryParams, cancellationToken);

        var entityList = await _repository.GetUserTransactionsAsync(userId, request.queryParams, cancellationToken);

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
