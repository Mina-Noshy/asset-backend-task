using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Account;
using Asset.Domain.Interfaces.Auth;

namespace Asset.Application.Services.Auth.Account;

public record ConfirmEmailCommand
    (ConfirmEmailDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;


public class ConfirmEmailCommandHandler(IAccountRepository _repository) : ICommandHandler<ConfirmEmailCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.ConfirmEmailAsync(request.requestDto.UserId, request.requestDto.Token, cancellationToken);
        if (result)
        {
            return new ApiResponse(ResultType.Success, "Email confirmed successfully. You can now log in to your account.");
        }

        return new ApiResponse(ResultType.Failure, "We couldn't find an account associated with the provided email address. Please ensure you've entered the correct email address.");
    }
}