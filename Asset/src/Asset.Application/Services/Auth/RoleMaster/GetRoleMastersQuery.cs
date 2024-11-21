using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.RoleMaster;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.RoleMaster;

public record GetRoleMastersQuery
    (QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetRoleMastersQueryHandler(IRoleMasterRepository _repository) : IQueryHandler<GetRoleMastersQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetRoleMastersQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.CountAsync(request.queryParams, cancellationToken);

        if (totalCount == 0)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.NoItemsFound);
        }

        var entityList = await _repository.GetPagedAsync(request.queryParams, cancellationToken);

        var entityListDto = entityList.Adapt<IEnumerable<RoleMasterDto>>();

        var pagedResponse = new PagedResponse<RoleMasterDto>(entityListDto, request.queryParams.PageNumber, request.queryParams.PageSize, totalCount);

        return new ApiResponse(ResultType.Success, pagedResponse);
    }
}
