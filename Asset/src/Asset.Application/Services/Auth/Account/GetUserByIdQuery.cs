using Asset.Application.Abstractions;
using Asset.Application.Common;
using Asset.Application.DTOs.Auth.Account;
using Asset.Domain.Interfaces.Auth;
using Mapster;

namespace Asset.Application.Services.Auth.Account;

public record GetUserByIdQuery
    (long id, CancellationToken cancellationToken = default) : IQuery<ApiResponse>;


public class GetUserByIdQueryHandler(IAccountRepository _repository) : IQueryHandler<GetUserByIdQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetUserByIdAsync(request.id, cancellationToken);
        if (entity != null)
        {
            var entityDto = entity.Adapt<UserMasterDto>();
            return new ApiResponse(ResultType.Success, entityDto);
        }
        return new ApiResponse(ResultType.Failure, $"Account with ID '{request.id}' was not found.");
    }
}