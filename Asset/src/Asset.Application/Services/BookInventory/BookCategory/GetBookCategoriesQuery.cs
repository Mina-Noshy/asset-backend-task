using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookCategory;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.BookInventory;
using Mapster;

namespace Asset.Application.Services.BookInventory.BookCategory;

public record GetBookCategoriesQuery
    (QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetBookCategorysQueryHandler(IBookCategoryRepository _repository) : IQueryHandler<GetBookCategoriesQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetBookCategoriesQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.CountAsync(request.queryParams, cancellationToken);

        var entityList = await _repository.GetPagedAsync(request.queryParams, cancellationToken);

        var entityListDto = entityList.Adapt<IEnumerable<BookCategoryDto>>();

        var pagedResponse = new PagedResponse<BookCategoryDto>(entityListDto, request.queryParams.PageNumber, request.queryParams.PageSize, totalCount);

        return new ApiResponse(ResultType.Success, pagedResponse);
    }
}
