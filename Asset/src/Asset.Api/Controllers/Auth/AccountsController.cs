using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.Auth.Account;
using Asset.Application.Services.Auth.Account;
using Asset.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Auth;

[ApiVersion("1.0")]
public class AccountsController : BaseV1AuthController
{
    [HttpPost("create-user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new CreateUserCommand(request, cancellationToken));

    [HttpGet("users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUsersAsync([FromQuery] QueryParams queryParams, CancellationToken cancellationToken)
        => await Mediator.Send(new GetUsersQuery(queryParams, cancellationToken));

    [HttpGet("user/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetUserByIdQuery(id, cancellationToken));

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new ConfirmEmailCommand(request, cancellationToken));

    [HttpPost("send-confirmation-email")]
    public async Task<IActionResult> SendConfirmationEmailAsync(string email, CancellationToken cancellationToken)
        => await Mediator.Send(new SendConfirmationEmailCommand(email, cancellationToken));

}
