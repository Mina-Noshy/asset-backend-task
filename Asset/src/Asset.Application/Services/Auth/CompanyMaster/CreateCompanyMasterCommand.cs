using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.CompanyMaster;
using Asset.Domain.Interfaces.Auth;
using FluentValidation;
using Mapster;
using CompanyMasterEntity = Asset.Domain.Entities.Auth.CompanyMaster;

namespace Asset.Application.Services.Auth.CompanyMaster;

public record CreateCompanyMasterCommand
    (CreateCompanyMasterDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class CreateCompanyMasterCommandHandler(ICompanyMasterRepository _repository) : ICommandHandler<CreateCompanyMasterCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateCompanyMasterCommand request, CancellationToken cancellationToken = default)
    {
        var entity = request.requestDto.Adapt<CompanyMasterEntity>();

        var result = await _repository.AddAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<CompanyMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulCreate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedCreate);
    }
}



public sealed class CreateCompanyMasterCommandValidator : AbstractValidator<CreateCompanyMasterCommand>
{
    public CreateCompanyMasterCommandValidator()
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
