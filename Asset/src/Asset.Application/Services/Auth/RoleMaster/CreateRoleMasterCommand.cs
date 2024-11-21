using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.RoleMaster;
using Asset.Domain.Interfaces.Auth;
using FluentValidation;
using Mapster;
using RoleMasterEntity = Asset.Domain.Entities.Auth.Identity.RoleMaster;

namespace Asset.Application.Services.Auth.RoleMaster;

public record CreateRoleMasterCommand
    (CreateRoleMasterDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class CreateRoleMasterCommandHandler(IRoleMasterRepository _repository) : ICommandHandler<CreateRoleMasterCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateRoleMasterCommand request, CancellationToken cancellationToken = default)
    {
        var entity = request.requestDto.Adapt<RoleMasterEntity>();

        var result = await _repository.AddAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<RoleMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulCreate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedCreate);
    }
}



public sealed class CreateRoleMasterCommandValidator : AbstractValidator<CreateRoleMasterCommand>
{
    public CreateRoleMasterCommandValidator()
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
