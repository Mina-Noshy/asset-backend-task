using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Auth;
using Asset.Domain.Constants;
using Asset.Domain.Entities.Auth;
using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Asset.Application.Services.Auth.Common;

public static class AuthHelper
{
    public static ApiResponse UnauthorizedResponse(string message)
    {
        return new ApiResponse(ResultType.UnAuthorized, message);
    }

    public static string? GetConfirmationMessage(UserMaster user, string userNameFromRequest)
    {
        if (!user.EmailConfirmed && !user.PhoneNumberConfirmed)
        {
            return "Please confirm your email or phone number first, And try again later.";
        }

        if (!user.EmailConfirmed && user.Email == userNameFromRequest)
        {
            return "Please confirm your email before proceeding.";
        }

        if (!user.PhoneNumberConfirmed && user.PhoneNumber == userNameFromRequest)
        {
            return "Please confirm your phone number first, And try again later.";
        }

        return null;
    }

    public static ApiResponse SuccessResponse(object data)
    {
        return new ApiResponse(ResultType.Success, data);
    }

    public static UserInfoDto ConfigureAndGetUserInfo(UserMaster user, string companyId, DateTime currentDateTime, List<string>? roles)
    {
        var refreshToken = new RefreshToken()
        {
            Token = GenerateRefreshToken(),
            ExpiresAt = currentDateTime.AddDays(int.Parse(ConfigurationHelper.GetJWT("RefreshTokenExpirationInDays"))),
            CreatedAt = currentDateTime
        };


        var userInfoDto = new UserInfoDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user?.UserName ?? throw new ArgumentNullException(nameof(user.UserName)),
            Email = user?.Email ?? throw new ArgumentNullException(nameof(user.UserName)),
            AccessToken = GenerateJwtToken(user, roles, companyId),
            RefreshToken = refreshToken.Token,
            TokenExpiration = refreshToken.ExpiresAt,
            Roles = roles
        };


        return userInfoDto;
    }










    private static string GenerateJwtToken(UserMaster user, List<string>? roles, string companyNo)
    {
        var roleClaims = new List<Claim>();

        if (roles != null)
        {
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(UserClaims.USER_ROLES, role));
            }
        }

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new ArgumentNullException(nameof(user.Email))),
                new Claim(UserClaims.USER_ID, user.Id.ToString()),
                new Claim(UserClaims.USERNAME, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                new Claim(UserClaims.USER_EMAIL, user.Email ?? throw new ArgumentNullException(nameof(user.Email))),
                new Claim(UserClaims.COMPANY_NO, companyNo)
        }
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationHelper.GetJWT("Secret")));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: ConfigurationHelper.GetJWT("Issuer"),
            audience: ConfigurationHelper.GetJWT("Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(ConfigurationHelper.GetJWT("ExpiryMinutes"))),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }


}
