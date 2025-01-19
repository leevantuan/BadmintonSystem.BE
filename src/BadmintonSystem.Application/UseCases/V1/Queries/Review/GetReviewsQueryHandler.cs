using System.Linq.Expressions;
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

public sealed class GetReviewsQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : IQueryHandler<Query.GetReviewsQuery, PagedResult<Response.ReviewResponse>>
{
    public async Task<Result<PagedResult<Response.ReviewResponse>>> Handle
    (Query.GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Review>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Review>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Review>.UpperPageSize
                    ? PagedResult<Domain.Entities.Review>.UpperPageSize
                    : request.Data.PageSize;

            string reviewsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Review)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Review)}""
                              WHERE ""{nameof(Domain.Entities.Review.Comment)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);

                reviewsQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            reviewsQuery = reviewsQuery.Remove(reviewsQuery.Length - 2);

            reviewsQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Review> reviews =
                await context.Review.FromSqlRaw(reviewsQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Review.CountAsync(cancellationToken);

            var reviewPagedResult =
                PagedResult<Domain.Entities.Review>.Create(reviews, PageIndex, PageSize, totalCount);

            PagedResult<Response.ReviewResponse>? result =
                mapper.Map<PagedResult<Response.ReviewResponse>>(reviewPagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Review> reviewsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? reviewRepository.FindAll()
                : reviewRepository.FindAll(x => x.Comment.Contains(request.Data.SearchTerm));

            reviewsQuery = request.Data.SortOrder == SortOrder.Descending
                ? reviewsQuery.OrderByDescending(GetSortColumnProperty(request))
                : reviewsQuery.OrderBy(GetSortColumnProperty(request));

            var reviews = await PagedResult<Domain.Entities.Review>.CreateAsync(reviewsQuery, request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.ReviewResponse>? result = mapper.Map<PagedResult<Response.ReviewResponse>>(reviews);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Review, object>> GetSortColumnProperty
    (
        Query.GetReviewsQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => review => review.Comment,
            _ => review => review.Id
        };
    }
}
