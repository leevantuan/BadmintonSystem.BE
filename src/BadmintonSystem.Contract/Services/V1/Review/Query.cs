using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Review;

public static class Query
{
    public record GetReviewsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ReviewResponse>>;

    public record GetReviewByIdQuery(Guid Id)
        : IQuery<Response.ReviewResponse>;

    public record GetReviewsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.ReviewDetailResponse>>;
}
