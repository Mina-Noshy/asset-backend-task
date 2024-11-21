namespace Asset.Application.DTOs.BookInventory.BookTransaction;

public record BorrowBookDto : IDto
{
    public long BookId { get; set; }
    public DateTime? DueDate { get; set; }
}
