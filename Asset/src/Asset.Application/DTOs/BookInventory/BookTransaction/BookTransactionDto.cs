namespace Asset.Application.DTOs.BookInventory.BookTransaction;

public record BookTransactionDto : IDto
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public long UserId { get; set; }
    public string BookTitle { get; set; }
    public string UserName { get; set; }

    public string TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool IsOverdue { get; set; }
}
