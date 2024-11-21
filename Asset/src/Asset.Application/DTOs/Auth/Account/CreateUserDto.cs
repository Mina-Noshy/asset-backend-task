using System.ComponentModel.DataAnnotations;

namespace Asset.Application.DTOs.Auth.Account;

public record CreateUserDto : IDto
{
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(50)]
    public string UserName { get; set; }
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    [MaxLength(100)]
    public string Password { get; set; }
    [MaxLength(20)]
    public string Role { get; set; }
}
