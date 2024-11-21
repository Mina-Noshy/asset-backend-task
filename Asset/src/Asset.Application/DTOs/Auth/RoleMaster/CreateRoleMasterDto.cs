namespace Asset.Application.DTOs.Auth.RoleMaster;

public record CreateRoleMasterDto : IDto
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string? ConcurrencyStamp { get; set; }
}
