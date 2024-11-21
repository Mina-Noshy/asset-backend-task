using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Account;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.Account;

public record GetUsersQuery
    (QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;


public class GetUsersQueryHandler(IAccountRepository _repository) : IQueryHandler<GetUsersQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.GetUsersCountAsync(request.queryParams, cancellationToken);

        if (totalCount == 0)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.NoItemsFound);
        }

        var entityList = await _repository.GetUsersPagedAsync(request.queryParams, cancellationToken);

        var entityListDto = entityList.Adapt<IEnumerable<UserMasterDto>>();

        var pagedResponse = new PagedResponse<UserMasterDto>(entityListDto, request.queryParams.PageNumber, request.queryParams.PageSize, totalCount);

        return new ApiResponse(ResultType.Success, pagedResponse);
    }
}