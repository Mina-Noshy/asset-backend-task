using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookMaster;
using Asset.Domain.Interfaces.BookInventory;
using FluentValidation;
using Mapster;
using BookMasterEntity = Asset.Domain.Entities.BookInventory.BookMaster;

namespace Asset.Application.Services.BookInventory.BookMaster;

public record CreateBookMasterCommand
    (CreateBookMasterDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class CreateBookMasterCommandHandler(IBookMasterRepository _repository) : ICommandHandler<CreateBookMasterCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateBookMasterCommand request, CancellationToken cancellationToken = default)
    {
        var entity = request.requestDto.Adapt<BookMasterEntity>();

        var result = await _repository.AddAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulCreate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedCreate);
    }
}



public sealed class CreateBookMasterCommandValidator : AbstractValidator<CreateBookMasterCommand>
{
    public CreateBookMasterCommandValidator()
    {
        RuleFor(x => x.requestDto.Title)
             .NotEmpty()
             .MinimumLength(3);

        RuleFor(x => x.requestDto.Author)
             .NotEmpty()
             .MinimumLength(3);
    }
}
