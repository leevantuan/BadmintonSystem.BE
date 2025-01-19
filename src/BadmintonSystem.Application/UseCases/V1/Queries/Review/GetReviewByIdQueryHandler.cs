using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Review;

public sealed class GetReviewByIdQueryHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : IQueryHandler<Query.GetReviewByIdQuery, Response.GetReviewDetailResponse>
{
    public async Task<Result<Response.GetReviewDetailResponse>> Handle
        (Query.GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Review review = await reviewRepository.FindByIdAsync(request.Id, cancellationToken)
                                        ?? throw new ReviewException.ReviewNotFoundException(request.Id);

        Response.GetReviewDetailResponse? result = mapper.Map<Response.GetReviewDetailResponse>(review);

        return Result.Success(result);
    }
}
