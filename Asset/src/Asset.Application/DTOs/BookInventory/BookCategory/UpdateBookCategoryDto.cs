namespace Asset.Application.DTOs.BookInventory.BookCategory;

public record UpdateBookCategoryDto : IDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
