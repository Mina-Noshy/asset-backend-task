using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.BookInventory.BookMaster;
using Asset.Application.Services.BookInventory.BookMaster;
using Asset.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Master;

[ApiVersion("2.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookMasterController : BaseV2BookInventoryController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookMasterDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new CreateBookMasterCommand(request, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetBookMastersQuery(queryParams, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetBookMasterByIdQuery(id, cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookMasterDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new UpdateBookMasterCommand(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new DeleteBookMasterCommand(id, cancellationToken));

}
