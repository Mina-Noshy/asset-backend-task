using Asset.Domain.Common;
using Asset.Domain.Entities.Auth.Identity;

namespace Asset.Domain.Interfaces.Auth;

public interface IAccountRepository
{
    Task<IEnumerable<UserMaster>> GetUsersPagedAsync(QueryParams queryParams, CancellationToken cancellationToken = default);
    Task<int> GetUsersCountAsync(QueryParams queryParams, CancellationToken cancellationToken = default);
    Task<UserMaster?> GetUserByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<UserMaster?> GetUserByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<UserMaster?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserMaster?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<UserMaster?> GetUserByTokenAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> CreateUserAsync(UserMaster user, string password, CancellationToken cancellationToken = default);
    Task<bool> ConfirmEmailAsync(long userId, string token, CancellationToken cancellationToken = default);
    Task<bool> SendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default);

}
