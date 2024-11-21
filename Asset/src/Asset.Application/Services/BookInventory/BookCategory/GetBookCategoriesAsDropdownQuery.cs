using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Domain.Interfaces.BookInventory;

namespace Asset.Application.Services.BookInventory.BookCategory;

public record GetBookCategoriesAsDropdownQuery
(CancellationToken cancellationToken = default) : IQuery<ApiResponse>;


public class GetBookCategoriesAsDropdownQueryHandler(IBookCategoryRepository _repository) : IQueryHandler<GetBookCategoriesAsDropdownQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetBookCategoriesAsDropdownQuery request, CancellationToken cancellationToken)
    {
        var entityListDto = await _repository.GetAllAsDropdownAsync(cancellationToken);

        return new ApiResponse(ResultType.Success, entityListDto);
    }
}
