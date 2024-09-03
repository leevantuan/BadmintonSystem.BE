using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Contract.Abstractions.Shared;
public class PagedResult<T>
{
    public const int UpperPageSize = 100;
    public const int DefaultPageSize = 10;
    public const int DefaultPageIndex = 1;

    private PagedResult(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    // If Right ==> Can next also Disable
    public bool HasNextPage => PageIndex * PageSize < TotalCount;

    // If Right ==> Can previou also Disable
    public bool HasPreviousPage => PageIndex > 1;

    public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        // If pageIndex <= 0 ==> Using Default
        // If pageSize <= 0 ==> Using Default
        // If pageSize >= 100 ==> Using Default
        pageIndex = pageIndex <= 0 ? DefaultPageIndex : pageIndex;
        pageSize = pageSize <= 0
            ? DefaultPageSize
            : pageSize > UpperPageSize
            ? UpperPageSize : pageSize;

        // Total
        var totalCount = await query.CountAsync();

        // Skip ==> Bỏ qua bản ghi trước đó
        // Take ==> Lấy bản ghi tiếp theo
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new(items, pageIndex, pageSize, totalCount);
    }

    public static PagedResult<T> Create(List<T> items, int pageIndex, int pageSize, int totalCount)
        => new(items, pageIndex, pageSize, totalCount);
}
