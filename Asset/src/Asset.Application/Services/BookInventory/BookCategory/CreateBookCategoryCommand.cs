using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookCategory;
using Asset.Domain.Interfaces.BookInventory;
using FluentValidation;
using Mapster;
using BookCategoryEntity = Asset.Domain.Entities.BookInventory.BookCategory;

namespace Asset.Application.Services.BookInventory.BookCategory;

public record CreateBookCategoryCommand
    (CreateBookCategoryDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class CreateBookCategoryCommandHandler(IBookCategoryRepository _repository) : ICommandHandler<CreateBookCategoryCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateBookCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var entity = request.requestDto.Adapt<BookCategoryEntity>();

        var result = await _repository.AddAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookCategoryDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulCreate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedCreate);
    }
}



public sealed class CreateBookCategoryCommandValidator : AbstractValidator<CreateBookCategoryCommand>
{
    public CreateBookCategoryCommandValidator()
    {
        RuleFor(x => x.requestDto.Name)
             .NotEmpty()
             .MinimumLength(3)
             .MaximumLength(50);
    }
}
