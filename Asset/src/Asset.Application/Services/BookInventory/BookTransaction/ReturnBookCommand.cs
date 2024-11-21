using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.BookInventory.BookTransaction;
using Asset.Domain.Common;
using Asset.Domain.Enums;
using Asset.Domain.Interfaces.BookInventory;
using Asset.Domain.Interfaces.Common;
using FluentValidation;
using Mapster;
using BookTransactionEntity = Asset.Domain.Entities.BookInventory.BookTransaction;

namespace Asset.Application.Services.BookInventory.BookTransaction;

public record ReturnBookCommand
    (long bookId, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class ReturnBookCommandHandler(IBookTransactionRepository _repository,
                    IBookMasterRepository _bookMasterRepository,
                    ICurrentUser _currentUser,
                    IDateTimeProvider _dateTimeProvider) : ICommandHandler<ReturnBookCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ReturnBookCommand request, CancellationToken cancellationToken = default)
    {
        long.TryParse(_currentUser.UserId, out long userId);

        var book = await _bookMasterRepository.GetByIdAsync(request.bookId, cancellationToken);
        if (book == null)
        {
            return new ApiResponse(ResultType.Failure, ApiMessage.ItemNotFound);
        }

        // OrderBy(x => x.Id)
        // Because if i have borrow thwo copies of same book
        // So i will return first one which i have borrowed
        var userBookTransactions = await _repository.GetUserBookTransactionsAsync(userId, request.bookId, new QueryParams(), cancellationToken);
        var borrowedBook = userBookTransactions.OrderBy(x => x.Id).FirstOrDefault(x => x.ReturnedDate == null);
        if (borrowedBook == null)
        {
            return new ApiResponse(ResultType.Failure, "You cannot return this book as it hasn't been borrowed or is already returned.");
        }

        var entity = new BookTransactionEntity()
        {
            Id = 0,
            BookId = request.bookId,
            UserId = userId,
            TransactionType = TransactionTypes.Returned,
            TransactionDate = _dateTimeProvider.CurrentDateTime,
            ReturnedDate = _dateTimeProvider.CurrentDateTime,
            IsOverdue = _dateTimeProvider.CurrentDateTime > borrowedBook.DueDate
        };

        // Begin transaction
        await _repository.BeginTransactionAsync(cancellationToken);

        var result = await _repository.AddAsync(entity, cancellationToken);
        if (result)
        {
            // Update borrowed one
            borrowedBook.ReturnedDate = _dateTimeProvider.CurrentDateTime;
            borrowedBook.IsOverdue = _dateTimeProvider.CurrentDateTime > borrowedBook.DueDate;

            result = await _repository.UpdateAsync(borrowedBook, cancellationToken);
        }
        if (result)
        {
            // Commit transaction
            await _repository.CommitTransactionAsync(cancellationToken);

            var entityDto = entity.Adapt<BookTransactionDto>();
            return new ApiResponse(ResultType.Success, ApiMessage.SuccessfulCreate, entityDto);
        }

        // Rollback transaction
        await _repository.RollbackTransactionAsync(cancellationToken);

        return new ApiResponse(ResultType.Failure, ApiMessage.FailedCreate);
    }
}



public sealed class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(x => x.bookId)
             .NotNull();
    }
}
