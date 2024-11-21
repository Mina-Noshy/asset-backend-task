namespace Asset.Application.DTOs.Auth.Auth;

public record UserInfoDto : IDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenExpiration { get; set; }
    public List<string>? Roles { get; set; }

}
