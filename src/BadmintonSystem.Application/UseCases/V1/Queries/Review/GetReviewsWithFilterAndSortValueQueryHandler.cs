using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Review;

public sealed class GetReviewsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : IQueryHandler<Query.GetReviewsWithFilterAndSortValueQuery, PagedResult<Response.ReviewDetailResponse>>
{
    public async Task<Result<PagedResult<Response.ReviewDetailResponse>>> Handle
        (Query.GetReviewsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Review>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Review>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Review>.UpperPageSize
                ? PagedResult<Domain.Entities.Review>.UpperPageSize
                : request.Data.PageSize;

        var reviewsQuery = new StringBuilder();

        reviewsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Review)}""
                             WHERE ""{nameof(Domain.Entities.Review.Comment)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                reviewsQuery.Append($@"AND ""{nameof(Domain.Entities.Review)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    reviewsQuery.Append($@"'%{value}%', ");
                }

                reviewsQuery.Length -= 2;

                reviewsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            reviewsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                reviewsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Review)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Review)}"".""{key}"" ASC, ");
            }

            reviewsQuery.Length -= 2;
        }

        reviewsQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");

        List<Domain.Entities.Review> reviews =
            await context.Review.FromSqlRaw(reviewsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = reviews.Count();

        var reviewPagedResult = PagedResult<Domain.Entities.Review>.Create(reviews, PageIndex, PageSize, totalCount);

        PagedResult<Response.ReviewDetailResponse>? result =
            mapper.Map<PagedResult<Response.ReviewDetailResponse>>(reviewPagedResult);

        return Result.Success(result);
    }
}
