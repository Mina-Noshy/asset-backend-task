using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Asset.Domain.Entities.Auth.Identity;

public class UserMaster : IdentityUser<long>
{

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    public bool IsActive { get; set; } = false;
    public bool IsBlocked { get; set; } = false;

    public virtual List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
