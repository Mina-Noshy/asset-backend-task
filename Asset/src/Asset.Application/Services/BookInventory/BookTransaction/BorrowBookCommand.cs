using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookTransaction;
using Asset.Domain.Enums;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;
using FluentValidation;
using Mapster;
using BookTransactionEntity = Asset.Domain.Entities.BookInventory.BookTransaction;


namespace Asset.Application.Services.BookInventory.BookTransaction;

public record BorrowBookCommand
    (BorrowBookDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class BorrowBookCommandHandler(IBookTransactionRepository _repository,
                    IBookMasterRepository _bookMasterRepository,
                    ICurrentUser _currentUser,
                    IDateTimeProvider _dateTimeProvider) : ICommandHandler<BorrowBookCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(BorrowBookCommand request, CancellationToken cancellationToken = default)
    {
        long.TryParse(_currentUser.UserId, out long userId);

        var book = await _bookMasterRepository.GetByIdAsync(request.requestDto.BookId, cancellationToken);
        if (book == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        var bookQuantity = book.Quantity;
        var borrowedCount = await _repository.BorrowedCountAsync(request.requestDto.BookId, cancellationToken);
        if (borrowedCount >= bookQuantity)
        {
            return new ApiResponse(ResultType.Failure, "All copies of this book are currently unavailable. Please wait until they are returned");
        }

        var entity = new BookTransactionEntity()
        {
            Id = 0,
            BookId = request.requestDto.BookId,
            UserId = userId,
            DueDate = request.requestDto.DueDate,
            TransactionType = TransactionTypes.Borrowed,
            TransactionDate = _dateTimeProvider.CurrentDateTime
        };

        var result = await _repository.AddAsync(entity, cancellationToken);
        if (result)
        {
            var entityDto = entity.Adapt<BookTransactionDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulCreate, entityDto);
        }
        return new ApiResponse(ResultType.Failure, ApiMessage.FailedCreate);
    }
}



public sealed class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(x => x.requestDto.BookId)
             .NotNull();
    }
}
