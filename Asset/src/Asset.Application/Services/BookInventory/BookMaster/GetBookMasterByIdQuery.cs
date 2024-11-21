using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookMaster;
using Asset.Domain.Interfaces.BookInventory;

namespace Asset.Application.Services.BookInventory.BookMaster;

public record GetBookMasterByIdQuery
    (long id, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetBookMasterByIdQueryHandler(IBookMasterRepository _repository) : IQueryHandler<GetBookMasterByIdQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetBookMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var entityDto = new BookMasterDto()
        {
            Id = entity.Id,
            CategoryId = entity.CategoryId,
            Category = entity.GetCategory.Name,
            Title = entity.Title,
            Author = entity.Author,
            Description = entity.Description,
            PublicationDate = entity.PublicationDate,
            Quantity = entity.Quantity
        };

        return new ApiResponse(ResultType.Success, entityDto);
    }
}
