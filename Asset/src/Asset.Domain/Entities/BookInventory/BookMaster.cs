using Asset.Domain.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset.Domain.Entities.BookInventory;

public class BookMaster : TEntity
{
    public long CategoryId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublicationDate { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }


    [ForeignKey(nameof(CategoryId))]
    public virtual BookCategory GetCategory { get; set; }
    public virtual ICollection<BookTransaction>? BookTransactions { get; set; }
}
