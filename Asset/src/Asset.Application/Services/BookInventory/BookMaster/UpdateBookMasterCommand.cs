using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookMaster;
using Asset.Domain.Interfaces.BookInventory;
using FluentValidation;
using Mapster;
using BookMasterEntity = Asset.Domain.Entities.BookInventory.BookMaster;

namespace Asset.Application.Services.BookInventory.BookMaster;

public record UpdateBookMasterCommand
    (long id, UpdateBookMasterDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;



public class UpdateBookMasterCommandHandler(IBookMasterRepository _repository) : ICommandHandler<UpdateBookMasterCommand, ApiResponse>
{

    public async Task<ApiResponse> Handle(UpdateBookMasterCommand request, CancellationToken cancellationToken)
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

        var entityToUpdate = request.requestDto.Adapt<BookMasterEntity>();
        entityToUpdate.CreatedBy = entity.CreatedBy;
        entityToUpdate.CreatedAt = entity.CreatedAt;

        var result = await _repository.UpdateAsync(entityToUpdate, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookMasterDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulUpdate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedUpdate);
    }
}


public sealed class UpdateBookMasterCommandValidator : AbstractValidator<UpdateBookMasterCommand>
{
    public UpdateBookMasterCommandValidator()
    {
        RuleFor(x => x.requestDto.Title)
             .NotEmpty()
             .MinimumLength(3);

        RuleFor(x => x.requestDto.Author)
             .NotEmpty()
             .MinimumLength(3);
    }
}