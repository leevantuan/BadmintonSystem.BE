using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Category;

public static class Query
{
    public record GetCategoriesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.CategoryResponse>>;

    public record GetCategoryByIdQuery(Guid Id)
        : IQuery<Response.CategoryResponse>;

    public record GetCategoriesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.CategoryDetailResponse>>;

    public record GetServicesByCategoryIdWithFilterAndSortValueQuery(
        Guid CategoryId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.GetServicesByCategoryIdResponse>>;
}
