namespace Asset.Domain.Interfaces.Common;

public interface ICurrentUser
{
    string UserId { get; }
    string Username { get; }
    string[] UserRoles { get; }
    string CompanyNo { get; }
}
