using Asset.Domain.Extensions;
using Asset.Domain.Interfaces.Common;
using Microsoft.AspNetCore.Http;

namespace Asset.Domain.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ?? throw new ApplicationException("User context is unavailable");

    public string Username =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUsername() ?? throw new ApplicationException("User context is unavailable");

    public string[] UserRoles =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserRoles() ?? throw new ApplicationException("User context is unavailable");

    public string CompanyNo =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetCompanyNo() ?? throw new ApplicationException("User context is unavailable");
}
