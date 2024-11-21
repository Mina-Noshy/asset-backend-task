using Asp.Versioning;
using Asset.Api.Controllers.Base;
using Asset.Application.DTOs.Auth.Auth;
using Asset.Application.Services.Auth.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset.Api.Controllers.Auth;

[ApiVersion("1.0")]
public class AuthController : BaseV1AuthController
{
    [HttpPost("get-token")]
    public async Task<IActionResult> GetTokenAsync(GetTokenDto request, CancellationToken cancellationToken)
            => await Mediator.Send(new GetTokenQuery(request, cancellationToken));

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new RefreshTokenCommand(request, cancellationToken));

    [HttpPost("revoke-token")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RevokeTokenAsync(TokenDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new RevokeTokenCommand(request, cancellationToken));

    [HttpPost("add-user-role")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddUserRoleAsync(UserToRoleDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new AddUserRoleCommand(request, cancellationToken));

    [HttpPost("remove-user-role")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveUserRoleAsync(UserToRoleDto request, CancellationToken cancellationToken)
        => await Mediator.Send(new RemoveUserRoleCommand(request, cancellationToken));

}
