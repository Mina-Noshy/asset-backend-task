using Asset.Domain.Entities.Shared;

namespace Asset.Domain.Entities.BookInventory;

public class BookCategory : TEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<BookMaster>? GetBooks { get; set; }
}
