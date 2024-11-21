using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookMaster;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.BookInventory;

namespace Asset.Application.Services.BookInventory.BookMaster;

public record GetBookMastersQuery
    (QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetBookMastersQueryHandler(IBookMasterRepository _repository) : IQueryHandler<GetBookMastersQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetBookMastersQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.CountAsync(request.queryParams, cancellationToken);

        var entityList = await _repository.GetPagedAsync(request.queryParams, cancellationToken);

        var entityListDto = entityList.Select(x => new BookMasterDto()
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            Category = x.GetCategory.Name,
            Title = x.Title,
            Author = x.Author,
            Description = x.Description,
            PublicationDate = x.PublicationDate,
            Quantity = x.Quantity
        });

        var pagedResponse = new PagedResponse<BookMasterDto>(entityListDto, request.queryParams.PageNumber, request.queryParams.PageSize, totalCount);

        return new ApiResponse(ResultType.Success, pagedResponse);
    }
}
