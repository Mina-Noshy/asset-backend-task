using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.Auth.RoleMaster;
using Asset.Application.Services.Auth.RoleMaster;
using Asset.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Auth;

[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RoleMasterController : BaseV1AuthController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleMasterDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new CreateRoleMasterCommand(request, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetRoleMastersQuery(queryParams, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetRoleMasterByIdQuery(id, cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleMasterDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new UpdateRoleMasterCommand(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new DeleteRoleMasterCommand(id, cancellationToken));

}
