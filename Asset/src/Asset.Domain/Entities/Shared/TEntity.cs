using System.ComponentModel.DataAnnotations;

namespace Asset.Domain.Entities.Shared;

public class TEntity
{

    [Key]
    public long Id { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
