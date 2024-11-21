namespace Asset.Application.DTOs.BookInventory.BookCategory;

public record BookCategoryDto : IDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
