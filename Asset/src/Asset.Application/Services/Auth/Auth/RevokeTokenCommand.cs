using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Auth;
using Asset.Domain.Interfaces.Auth;
using Asset.Domain.Interfaces.Common;

namespace Asset.Application.Services.Auth.Auth;

public record RevokeTokenCommand
    (TokenDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;

public class RevokeTokenCommandHandler(IAuthRepository _repository,
    IAccountRepository _accountRepository,
    IDateTimeProvider _dateTimeProvider) : ICommandHandler<RevokeTokenCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetUserByTokenAsync(request.requestDto.Token, cancellationToken);
        if (user is null)
        {
            return new ApiResponse(ResultType.Failure, "Invalid token.");
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == request.requestDto.Token);
        if (refreshToken.IsActive == false)
        {
            return new ApiResponse(ResultType.Failure, "Token already expired.");
        }

        refreshToken.RevokedAt = _dateTimeProvider.CurrentDateTime;
        await _repository.UpdateUser(user);

        return new ApiResponse(ResultType.Success, "Token revoked successfully.");
    }
}