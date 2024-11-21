using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.CompanyMaster;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.CompanyMaster;

public record GetCompanyMasterByIdQuery
    (long id, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetCompanyMasterByIdQueryHandler(ICompanyMasterRepository _repository) : IQueryHandler<GetCompanyMasterByIdQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetCompanyMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var entityDto = entity.Adapt<CompanyMasterDto>();

        return new ApiResponse(ResultType.Success, entityDto);
    }
}
