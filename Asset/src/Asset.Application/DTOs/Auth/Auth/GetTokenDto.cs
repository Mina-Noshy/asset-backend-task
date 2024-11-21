namespace Asset.Application.DTOs.Auth.Auth;

public record GetTokenDto : IDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string CompanyNo { get; set; } = string.Empty;
}
