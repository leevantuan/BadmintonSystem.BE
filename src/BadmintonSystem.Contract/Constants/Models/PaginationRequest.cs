using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Constants.Models;
public class PaginationRequest
{
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public SortOrder? SortOrder { get; set; }
    public IDictionary<string, SortOrder>? SortColumnAndOrder { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
