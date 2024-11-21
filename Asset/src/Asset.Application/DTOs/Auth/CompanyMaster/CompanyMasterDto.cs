namespace Asset.Application.DTOs.Auth.CompanyMaster;

public record CompanyMasterDto : IDto
{
    public long Id { get; set; }
    public string CompanyNo { get; set; }
    public string CompanyName { get; set; }

    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }

    public string? RegistrationNumber { get; set; }
    public string? ZipCode { get; set; }
    public string? Industry { get; set; }
    public string? Sector { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? Logo { get; set; }
    public string PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string Email { get; set; }
    public string? CEO { get; set; }
    public DateTime? Founded { get; set; }
    public int? EmployeeCount { get; set; }
    public string? Headquarters { get; set; }
    public string? IndustryType { get; set; } // e.g. Public, Private, Non-Profit, etc.
    public string? Type { get; set; } // e.g. Corporation, Partnership, Sole Proprietorship, etc.
    public string? Status { get; set; } // e.g. Active, Inactive, Bankrupt, etc.
    public string? Notes { get; set; } // Additional notes about the company

    public string? FrontColor { get; set; } // To change the front-end style base on eatch company
    public string? BackColor { get; set; } // To change the front-end style base on eatch company
    public string? Background { get; set; } // To change the front-end style base on eatch company

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
