using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookMaster;
using Asset.Domain.Interfaces.BookInventory;
using Mapster;

namespace Asset.Application.Services.BookInventory.BookMaster;

public record DeleteBookMasterCommand
    (long id, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class DeleteBookMasterCommandHandler(IBookMasterRepository _repository) : ICommandHandler<DeleteBookMasterCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(DeleteBookMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var result = await _repository.DeleteAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulDelete, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedDelete);
    }
}
