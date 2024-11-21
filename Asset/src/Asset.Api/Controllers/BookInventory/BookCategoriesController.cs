using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.BookInventory.BookCategory;
using Asset.Application.Services.BookInventory.BookCategory;
using Asset.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Master;

[ApiVersion("2.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookCategoriesController : BaseV2BookInventoryController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookCategoryDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new CreateBookCategoryCommand(request, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetBookCategoriesQuery(queryParams, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetBookCategoryByIdQuery(id, cancellationToken));

    [HttpGet("dropdown")]
    public async Task<IActionResult> GetAsDropdown(CancellationToken cancellationToken)
        => await Mediator.Send(new GetBookCategoriesAsDropdownQuery(cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookCategoryDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new UpdateBookCategoryCommand(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new DeleteBookCategoryCommand(id, cancellationToken));

}
