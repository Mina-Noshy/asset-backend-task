using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.RoleMaster;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.RoleMaster;

public record DeleteRoleMasterCommand
    (long id, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class DeleteRoleMasterCommandHandler(IRoleMasterRepository _repository) : ICommandHandler<DeleteRoleMasterCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(DeleteRoleMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var result = await _repository.DeleteAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<RoleMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulDelete, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedDelete);
    }
}
