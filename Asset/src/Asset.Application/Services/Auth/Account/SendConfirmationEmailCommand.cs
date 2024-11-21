using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Domain.Interfaces.Auth;

namespace Asset.Application.Services.Auth.Account;

public record SendConfirmationEmailCommand
    (string email, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;

public class SendConfirmationEmailCommandHandler(IAccountRepository _repository) : ICommandHandler<SendConfirmationEmailCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SendConfirmationEmailAsync(request.email, cancellationToken);
        if (result)
        {
            return new ApiResponse(ResultType.Success, "Confirmation email has been successfully dispatched, Please check your inbox");
        }
        return new ApiResponse(ResultType.Failure, $"We couldn't find an account associated with the provided email address '{request.email}'.");
    }
}
