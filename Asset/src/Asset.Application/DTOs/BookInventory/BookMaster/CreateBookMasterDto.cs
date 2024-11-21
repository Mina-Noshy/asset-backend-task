namespace Asset.Application.DTOs.BookInventory.BookMaster;

public record CreateBookMasterDto : IDto
{
    public long CategoryId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublicationDate { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
}
