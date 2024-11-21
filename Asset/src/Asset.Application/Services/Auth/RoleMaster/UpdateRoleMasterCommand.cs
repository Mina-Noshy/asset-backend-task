using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.RoleMaster;
using Asset.Domain.Interfaces.Auth;
using FluentValidation;
using Mapster;
using RoleMasterEntity = Asset.Domain.Entities.Auth.Identity.RoleMaster;

namespace Asset.Application.Services.Auth.RoleMaster;

public record UpdateRoleMasterCommand
    (long id, UpdateRoleMasterDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;



public class UpdateRoleMasterCommandHandler(IRoleMasterRepository _repository) : ICommandHandler<UpdateRoleMasterCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(UpdateRoleMasterCommand request, CancellationToken cancellationToken)
    {
        if (request.id != request.requestDto.Id)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.EntityIdMismatch);
        }

        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var entityToUpdate = request.requestDto.Adapt<RoleMasterEntity>();

        var result = await _repository.UpdateAsync(entityToUpdate, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<RoleMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulUpdate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedUpdate);
    }
}


public sealed class UpdateRoleMasterCommandValidator : AbstractValidator<UpdateRoleMasterCommand>
{
    public UpdateRoleMasterCommandValidator()
    {
        RuleFor(x => x.requestDto.Name)
             .NotEmpty()
             .MinimumLength(3)
             .MaximumLength(50);

        RuleFor(x => x.requestDto.NormalizedName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(50);
    }
}