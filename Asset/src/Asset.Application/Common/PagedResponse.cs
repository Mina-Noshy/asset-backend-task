using Asset.Application.DTOs;

namespace Asset.Application.Common;

public class PagedResponse<T> : IDto where T : IDto
{
    public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
    }

    public IEnumerable<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
