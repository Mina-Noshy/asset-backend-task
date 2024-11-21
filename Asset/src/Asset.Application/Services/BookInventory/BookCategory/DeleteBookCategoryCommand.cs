using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookCategory;
using Asset.Domain.Interfaces.BookInventory;
using Mapster;

namespace Asset.Application.Services.BookInventory.BookCategory;

public record DeleteBookCategoryCommand
    (long id, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class DeleteBookCategoryCommandHandler(IBookCategoryRepository _repository) : ICommandHandler<DeleteBookCategoryCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(DeleteBookCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var result = await _repository.DeleteAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookCategoryDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulDelete, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedDelete);
    }
}
