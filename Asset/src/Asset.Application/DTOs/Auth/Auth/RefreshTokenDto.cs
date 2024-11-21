namespace Asset.Application.DTOs.Auth.Auth;

public record RefreshTokenDto : IDto
{
    public string Token { get; set; }
    public string CompanyNo { get; set; } = string.Empty;
}
