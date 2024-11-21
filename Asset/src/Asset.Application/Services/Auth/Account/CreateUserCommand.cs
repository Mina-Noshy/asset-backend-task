using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Account;
using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Interfaces.Auth;
using FluentValidation;
using Mapster;

namespace Asset.Application.Services.Auth.Account;

public record CreateUserCommand
    (CreateUserDto requestDto, CancellationToken cancellationToken = default) : ICommand<ApiResponse>;

public class CreateUserCommandHandler(IAccountRepository _repository, IAuthRepository _authRepository) : ICommandHandler<CreateUserCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.GetUserByNameAsync(request.requestDto.UserName) != null)
        {
            return new ApiResponse(ResultType.Failure, $"Failed to create the user, Username '{request.requestDto.UserName}' already exists");
        }
        else if (await _repository.GetUserByEmailAsync(request.requestDto.Email) != null)
        {
            return new ApiResponse(ResultType.Failure, $"Failed to create the user, Email '{request.requestDto.Email}' already exists");
        }
        else if (await _repository.GetUserByPhoneNumberAsync(request.requestDto.PhoneNumber) != null)
        {
            return new ApiResponse(ResultType.Failure, $"Failed to create the user, PhoneNumber '{request.requestDto.PhoneNumber}' already exists");
        }

        var entity = request.requestDto.Adapt<UserMaster>();

        var result = await _repository.CreateUserAsync(entity, request.requestDto.Password, cancellationToken);

        if (result)
        {
            // Add user to role
            await _authRepository.AddUserRoleAsync(entity, request.requestDto.Role);

            // We can enable after adding the correct 'SMTP' configration in 'SMTP.json' file
            // Send confirmation email
            //await _repository.SendConfirmationEmailAsync(request.requestDto.Email);

            var entityDto = entity.Adapt<UserMasterDto>();
            entityDto.PasswordHash = entityDto.SecurityStamp = "***encrypted***";
            return new ApiResponse(ResultType.Success, "The user has been created successfully, Please check your email inbox", entityDto);
        }

        return new ApiResponse(ResultType.Failure, "Failed to create the user. Please check your input and try again later");
    }
}

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.requestDto.FirstName)
             .NotEmpty()
             .MinimumLength(3)
             .MaximumLength(50);

        RuleFor(x => x.requestDto.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.requestDto.UserName)
           .NotEmpty()
           .MinimumLength(3)
           .MaximumLength(50);

        RuleFor(x => x.requestDto.Email)
           .NotEmpty()
           .MinimumLength(5)
           .MaximumLength(100);

        RuleFor(x => x.requestDto.PhoneNumber)
           .NotEmpty()
           .MinimumLength(8)
           .MaximumLength(15);

        RuleFor(x => x.requestDto.Password)
          .NotEmpty()
          .MinimumLength(6)
          .MaximumLength(200);

        RuleFor(x => x.requestDto.Role)
          .NotEmpty()
          .MinimumLength(2)
          .MaximumLength(20);
    }
}