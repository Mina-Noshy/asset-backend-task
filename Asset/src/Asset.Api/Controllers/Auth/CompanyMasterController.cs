using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.Auth.CompanyMaster;
using Asset.Application.Services.Auth.CompanyMaster;
using Asset.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Auth;

[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CompanyMasterController : BaseV1AuthController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyMasterDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new CreateCompanyMasterCommand(request, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetCompanyMastersQuery(queryParams, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetCompanyMasterByIdQuery(id, cancellationToken));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyMasterDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new UpdateCompanyMasterCommand(id, request, cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await Mediator.Send(new DeleteCompanyMasterCommand(id, cancellationToken));

}
