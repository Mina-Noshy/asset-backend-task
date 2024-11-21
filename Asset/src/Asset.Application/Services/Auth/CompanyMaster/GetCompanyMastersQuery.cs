using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.CompanyMaster;
using Asset.Domain.Common;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.CompanyMaster;

public record GetCompanyMastersQuery
    (QueryParams queryParams, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetCompanyMastersQueryHandler(ICompanyMasterRepository _repository) : IQueryHandler<GetCompanyMastersQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetCompanyMastersQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.CountAsync(request.queryParams, cancellationToken);

        if (totalCount == 0)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.NoItemsFound);
        }

        var entityList = await _repository.GetPagedAsync(request.queryParams, cancellationToken);

        var entityListDto = entityList.Adapt<IEnumerable<CompanyMasterDto>>();

        var pagedResponse = new PagedResponse<CompanyMasterDto>(entityListDto, request.queryParams.PageNumber, request.queryParams.PageSize, totalCount);

        return new ApiResponse(ResultType.Success, pagedResponse);
    }
}
