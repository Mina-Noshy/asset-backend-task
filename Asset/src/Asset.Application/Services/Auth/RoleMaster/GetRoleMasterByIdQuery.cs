using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.RoleMaster;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.RoleMaster;

public record GetRoleMasterByIdQuery
    (long id, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;

public class GetRoleMasterByIdQueryHandler(IRoleMasterRepository _repository) : IQueryHandler<GetRoleMasterByIdQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetRoleMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var entityDto = entity.Adapt<RoleMasterDto>();

        return new ApiResponse(ResultType.Success, entityDto);
    }
}
