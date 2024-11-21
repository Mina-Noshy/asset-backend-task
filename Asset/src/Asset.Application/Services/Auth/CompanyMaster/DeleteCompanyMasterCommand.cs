using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.CompanyMaster;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.CompanyMaster;

public record DeleteCompanyMasterCommand
    (long id, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class DeleteCompanyMasterCommandHandler(ICompanyMasterRepository _repository) : ICommandHandler<DeleteCompanyMasterCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(DeleteCompanyMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var result = await _repository.DeleteAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<CompanyMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulDelete, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedDelete);
    }
}
