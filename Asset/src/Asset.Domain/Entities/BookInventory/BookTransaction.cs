using Asset.Domain.Entities.Auth.Identity;
using Asset.Domain.Entities.Shared;
using Asset.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset.Domain.Entities.BookInventory;

public class BookTransaction : TEntity
{
    public long BookId { get; set; }
    public long UserId { get; set; }

    public TransactionTypes TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool IsOverdue { get; set; }  // Only applicable when ReturnedDate is set


    [ForeignKey(nameof(UserId))]
    public virtual UserMaster GetUser { get; set; }

    [ForeignKey(nameof(BookId))]
    public virtual BookMaster GetBook { get; set; }
}
