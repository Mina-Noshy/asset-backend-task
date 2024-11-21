using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.CompanyMaster;
using Asset.Domain.Interfaces.Auth;
using FluentValidation;
using Mapster;
using CompanyMasterEntity = Asset.Domain.Entities.Auth.CompanyMaster;

namespace Asset.Application.Services.Auth.CompanyMaster;

public record UpdateCompanyMasterCommand
    (long id, UpdateCompanyMasterDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;



public class UpdateCompanyMasterCommandHandler(ICompanyMasterRepository _repository) : ICommandHandler<UpdateCompanyMasterCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(UpdateCompanyMasterCommand request, CancellationToken cancellationToken)
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

        var entityToUpdate = request.requestDto.Adapt<CompanyMasterEntity>();
        entityToUpdate.CreatedBy = entity.CreatedBy;
        entityToUpdate.CreatedAt = entity.CreatedAt;

        var result = await _repository.UpdateAsync(entityToUpdate, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<CompanyMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulUpdate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedUpdate);
    }
}


public sealed class UpdateCompanyMasterCommandValidator : AbstractValidator<UpdateCompanyMasterCommand>
{
    public UpdateCompanyMasterCommandValidator()
    {
        RuleFor(x => x.requestDto.CompanyNo)
             .NotEmpty()
             .MinimumLength(3)
             .MaximumLength(50);

        RuleFor(x => x.requestDto.CompanyName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(50);
    }
}