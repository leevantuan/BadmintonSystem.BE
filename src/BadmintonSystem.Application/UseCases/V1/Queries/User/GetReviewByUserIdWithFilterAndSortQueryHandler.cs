using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetReviewByUserIdWithFilterAndSortQueryHandler(
    ApplicationDbContext context,
    IMapper mapper)
    : IQueryHandler<Query.GetReviewsByUserIdWithFilterAndSortQuery, PagedResult<Response.ReviewByUserResponse>>
{
    public async Task<Result<PagedResult<Response.ReviewByUserResponse>>> Handle
        (Query.GetReviewsByUserIdWithFilterAndSortQuery request, CancellationToken cancellationToken)
    {
        // Pagination
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Review>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Review>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Review>.UpperPageSize
                ? PagedResult<Domain.Entities.Review>.UpperPageSize
                : request.Data.PageSize;

        // Query result
        var query = from review in context.Review
            join reviewImage in context.ReviewImage
                on review.Id equals reviewImage.ReviewId
                into reviewImages
            from reviewImage in reviewImages.DefaultIfEmpty()
            where review.UserId == request.UserId
                  && review.IsDeleted == false
                  && reviewImage.IsDeleted == false
            select new { review, reviewImage };

        // Group by
        IQueryable<Response.ReviewByUserResponse> queryGrouped = query.AsNoTracking()
            .GroupBy(x => x.review.Id)
            .Select(x => new Response.ReviewByUserResponse
            {
                Id = x.Key,
                Comment = x.First().review.Comment,
                RatingValue = x.First().review.RatingValue,
                UserId = x.First().review.UserId,
                ClubId = x.First().review.ClubId,
                ReviewImages = x.Select(x => new Contract.Services.V1.ReviewImage.Response.ReviewImageDetailResponse
                {
                    Id = x.reviewImage.Id,
                    ImageLink = x.reviewImage.ImageLink,
                    ReviewId = x.review.Id
                }).ToList()
            });

        var resultPaged =
            await PagedResult<Response.ReviewByUserResponse>.CreateAsync(
                queryGrouped,
                pageIndex,
                pageSize);

        return Result.Success(resultPaged);
    }
}
