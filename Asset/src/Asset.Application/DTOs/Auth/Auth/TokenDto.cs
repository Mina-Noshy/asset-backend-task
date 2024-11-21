namespace Asset.Application.DTOs.Auth.Auth;

public record TokenDto : IDto
{
    public string Token { get; set; }
}
