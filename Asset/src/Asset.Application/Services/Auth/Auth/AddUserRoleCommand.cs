using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Auth;
using Asset.Domain.Interfaces.Auth;

namespace Asset.Application.Services.Auth.Auth;

public record AddUserRoleCommand
    (UserToRoleDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;

public class AddUserRoleCommandHandler(IAuthRepository _repository,
    IAccountRepository _accountRepository,
    IRoleMasterRepository _roleMasterRepository) : ICommandHandler<AddUserRoleCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetUserByIdAsync(request.requestDto.UserId, cancellationToken);
        if (user == null)
        {
            return new ApiResponse(ResultType.Failure, $"User with ID '{request.requestDto.UserId}' was not found.");
        }

        var role = await _roleMasterRepository.GetRoleByNameAsync(request.requestDto.Role, cancellationToken);
        if (role == null)
        {
            return new ApiResponse(ResultType.Failure, $"Role with name '{request.requestDto.Role}' was not found.");
        }

        var userRoles = await _roleMasterRepository.GetUserRolesAsync(user);
        if (userRoles != null && userRoles.Any(x => x == request.requestDto.Role))
        {
            return new ApiResponse(ResultType.Failure, $"Role with name '{request.requestDto.Role}' is already assigned to the user '{user.UserName}'.");
        }

        var result = await _repository.AddUserRoleAsync(user, request.requestDto.Role, cancellationToken);
        if (result.Succeeded == false)
        {
            return new ApiResponse(ResultType.Failure, "Something went wrong");
        }

        return new ApiResponse(ResultType.Success, $"Role '{request.requestDto.Role}' has been successfully added to the user '{user.UserName}'.");
    }
}