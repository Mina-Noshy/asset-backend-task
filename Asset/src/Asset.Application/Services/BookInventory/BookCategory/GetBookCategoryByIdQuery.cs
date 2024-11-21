using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookCategory;
using Asset.Domain.Interfaces.BookInventory;
using Mapster;

namespace Asset.Application.Services.BookInventory.BookCategory;

public record GetBookCategoryByIdQuery
    (long id, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetBookCategoryByIdQueryHandler(IBookCategoryRepository _repository) : IQueryHandler<GetBookCategoryByIdQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetBookCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var entityDto = entity.Adapt<BookCategoryDto>();

        return new ApiResponse(ResultType.Success, entityDto);
    }
}
