namespace Asset.Application.DTOs.BookInventory.BookCategory;

public record CreateBookCategoryDto : IDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
