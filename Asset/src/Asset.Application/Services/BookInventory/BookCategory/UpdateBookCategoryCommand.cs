using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookCategory;
using Asset.Domain.Interfaces.BookInventory;
using FluentValidation;
using Mapster;
using BookCategoryEntity = Asset.Domain.Entities.BookInventory.BookCategory;

namespace Asset.Application.Services.BookInventory.BookCategory;

public record UpdateBookCategoryCommand
    (long id, UpdateBookCategoryDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;



public class UpdateBookCategoryCommandHandler(IBookCategoryRepository _repository) : ICommandHandler<UpdateBookCategoryCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(UpdateBookCategoryCommand request, CancellationToken cancellationToken)
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

        var entityToUpdate = request.requestDto.Adapt<BookCategoryEntity>();
        entityToUpdate.CreatedBy = entity.CreatedBy;
        entityToUpdate.CreatedAt = entity.CreatedAt;

        var result = await _repository.UpdateAsync(entityToUpdate, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookCategoryDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulUpdate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedUpdate);
    }
}


public sealed class UpdateBookCategoryCommandValidator : AbstractValidator<UpdateBookCategoryCommand>
{
    public UpdateBookCategoryCommandValidator()
    {
        RuleFor(x => x.requestDto.Name)
             .NotEmpty()
             .MinimumLength(3)
             .MaximumLength(50);
    }
}