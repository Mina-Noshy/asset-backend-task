using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.BookInventory.BookTransaction;
using Asset.Application.Services.BookInventory.BookTransaction;
using Asset.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Master;

[ApiVersion("2.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookTransactionsController : BaseV2BookInventoryController
{
    [HttpPost("borrow")]
    public async Task<IActionResult> Create([FromBody] BorrowBookDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new BorrowBookCommand(request, cancellationToken));

    [HttpPost("return/{bookId}")]
    public async Task<IActionResult> Create(long bookId, CancellationToken cancellationToken)
            => await Mediator.Send(new ReturnBookCommand(bookId, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetAllTransactionsQuery(queryParams, cancellationToken));






    [HttpGet("user-transactions")]
    public async Task<IActionResult> GetAllUserTransactions([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetUserTransactionsQuery(queryParams, cancellationToken));

    [HttpGet("book-transactions/{bookId:long}")]
    public async Task<IActionResult> GetAllBookTransactions(long bookId, [FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
            => await Mediator.Send(new GetBookTransactionsQuery(bookId, queryParams, cancellationToken));

    [HttpGet("user-book-transactions/{bookId:long}")]
    public async Task<IActionResult> GetAllUserBookTransactions(long bookId, [FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
            => await Mediator.Send(new GetUserBookTransactionsQuery(bookId, queryParams, cancellationToken));

}
