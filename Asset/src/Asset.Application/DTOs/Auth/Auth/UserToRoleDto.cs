namespace Asset.Application.DTOs.Auth.Auth;

public record UserToRoleDto : IDto
{
    public long UserId { get; set; }
    public string Role { get; set; }
}
