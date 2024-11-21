using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Auth;
using Asset.Application.Services.Auth.Common;
using Asset.Domain.Entities.Auth;
using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Interfaces.Auth;
using Asset.Domain.Interfaces.Common;

namespace Asset.Application.Services.Auth.Auth;

public record RefreshTokenCommand
    (RefreshTokenDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;

public class RefreshTokenCommandHandler(IAuthRepository _repository,
    IAccountRepository _accountRepository,
    IRoleMasterRepository _roleMasterRepository,
    IDateTimeProvider _dateTimeProvider) : ICommandHandler<RefreshTokenCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetUserByTokenAsync(request.requestDto.Token, cancellationToken);

        if (user == null)
        {
            return AuthHelper.UnauthorizedResponse("Invalid token.");
        }

        if (!user.IsActive)
        {
            return AuthHelper.UnauthorizedResponse("Your account is inactive, Please contact support.");
        }

        if (user.IsBlocked)
        {
            return AuthHelper.UnauthorizedResponse("Your account is blocked, Please contact support.");
        }


        var refreshToken = user.RefreshTokens.Single(t => t.Token == request.requestDto.Token);

        if (!refreshToken.IsActive)
        {
            return AuthHelper.UnauthorizedResponse("Inactive token.");
        }

        var userInfo = await ConfigureAndGetUserInfo(user, request.requestDto.CompanyNo, cancellationToken);
        return AuthHelper.SuccessResponse(userInfo);
    }












    private async Task<UserInfoDto> ConfigureAndGetUserInfo(UserMaster user, string companyNo, CancellationToken cancellationToken = default)
    {

        var roles = (await _roleMasterRepository.GetUserRolesAsync(user, cancellationToken))?.ToList();

        var userInfoDto = AuthHelper.ConfigureAndGetUserInfo(user, companyNo, _dateTimeProvider.CurrentDateTime, roles);

        var refreshToken = new RefreshToken()
        {
            Token = userInfoDto.RefreshToken,
            ExpiresAt = userInfoDto.TokenExpiration,
            CreatedAt = _dateTimeProvider.CurrentDateTime
        };

        // Revoke all old active tokens
        foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
        {
            token.RevokedAt = _dateTimeProvider.CurrentDateTime; // Mark token as revoked
        }

        // Add new token
        user.RefreshTokens.Add(refreshToken);
        await _repository.UpdateUser(user, cancellationToken);

        return userInfoDto;
    }

}