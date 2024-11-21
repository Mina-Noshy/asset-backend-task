namespace Asset.Domain.Common;

public class QueryParams
{

    public string SearchTerm { get; set; } = string.Empty;
    public string SortColumn { get; set; } = "Id";
    public bool Ascending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
