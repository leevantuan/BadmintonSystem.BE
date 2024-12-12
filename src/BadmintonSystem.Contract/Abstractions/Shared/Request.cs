using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;

namespace BadmintonSystem.Contract.Abstractions.Shared;

public static class Request
{
    public class PagedRequest
    {
        public string? SearchTerm { get; set; }

        public string? SortColumnAndOrder { get; set; }

        public string? SortColumn { get; set; }

        public string? SortOrder { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class PagedQueryRequest
    {
        public PagedQueryRequest(PagedRequest pagedRequest)
        {
            SearchTerm = pagedRequest.SearchTerm;
            SortColumn = pagedRequest.SortColumn;
            SortOrder = SortOrderExtension.ConvertStringToSortOrderWithOneColumn(pagedRequest.SortOrder);
            SortColumnAndOrder =
                SortOrderExtension.ConvertStringToSortOrderWithMultipleColumn(pagedRequest.SortColumnAndOrder);
            PageIndex = pagedRequest.PageIndex;
            PageSize = pagedRequest.PageSize;
        }

        public string? SearchTerm { get; set; }

        public string? SortColumn { get; set; }

        public SortOrder? SortOrder { get; set; }

        public IDictionary<string, SortOrder>? SortColumnAndOrder { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public class PagedFilterAndSortRequest
    {
        public string? SearchTerm { get; set; }

        public string? FilterColumnAndMultipleValue { get; set; }

        public string? SortColumnAndOrder { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class PagedFilterAndSortQueryRequest
    {
        public PagedFilterAndSortQueryRequest
        (
            PagedFilterAndSortRequest pagedFilterAndSortRequest)
        {
            SearchTerm = pagedFilterAndSortRequest.SearchTerm;
            FilterColumnAndMultipleValue =
                FilterValueExtension.ConvertStringToFilterByMultipleValueWithMultipleColumn(
                    pagedFilterAndSortRequest.FilterColumnAndMultipleValue);
            SortColumnAndOrder =
                SortOrderExtension.ConvertStringToSortOrderWithMultipleColumn(pagedFilterAndSortRequest
                    .SortColumnAndOrder);
            PageIndex = pagedFilterAndSortRequest.PageIndex;
            PageSize = pagedFilterAndSortRequest.PageSize;
        }

        public string? SearchTerm { get; set; }

        public IDictionary<string, List<string>>? FilterColumnAndMultipleValue { get; set; }

        public IDictionary<string, SortOrder>? SortColumnAndOrder { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
