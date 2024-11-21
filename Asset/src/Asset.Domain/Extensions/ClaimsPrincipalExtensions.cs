using Asset.Domain.Constants;
using System.Security.Claims;

namespace Asset.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(UserClaims.USER_ID);
        return userId ?? throw new ApplicationException("User id is unavailable");
    }

    public static string GetUsername(this ClaimsPrincipal? principal)
    {
        var username = principal?.FindFirstValue(UserClaims.USERNAME);
        return username ?? throw new ApplicationException("Username is unavailable");
    }

    public static string[] GetUserRoles(this ClaimsPrincipal? principal)
    {
        if (principal == null)
        {
            throw new ApplicationException("ClaimsPrincipal is null");
        }

        // Collect all claims with the USER_ROLES type
        var roles = principal.Claims
            .Where(c => c.Type == UserClaims.USER_ROLES)
            .Select(c => c.Value)
            .ToArray();

        if (roles.Length == 0)
        {
            //return Array.Empty<string>(); 
            throw new ApplicationException("User roles are unavailable");
        }

        return roles;
    }

    public static string GetCompanyNo(this ClaimsPrincipal? principal)
    {
        var companyNo = principal?.FindFirstValue(UserClaims.COMPANY_NO);
        return companyNo ?? throw new ApplicationException("Company number is unavailable");
    }
}

