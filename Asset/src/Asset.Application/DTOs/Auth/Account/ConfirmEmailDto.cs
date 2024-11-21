namespace Asset.Application.DTOs.Auth.Account;

public record ConfirmEmailDto : IDto
{
    public long UserId { get; set; }
    public string Token { get; set; }
}
