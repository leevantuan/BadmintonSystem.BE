using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Services.V2.Category;
public static class Query
{
    public record GetCategoryByIdQuery(Guid Id) : IQuery<Response.CategoryResponse>;
    public record GetAllCategory(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<Response.CategoryResponse>>;
}

