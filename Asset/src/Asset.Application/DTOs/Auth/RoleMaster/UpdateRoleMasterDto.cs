﻿namespace Asset.Application.DTOs.Auth.RoleMaster;

public record UpdateRoleMasterDto : IDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string? ConcurrencyStamp { get; set; }
}
