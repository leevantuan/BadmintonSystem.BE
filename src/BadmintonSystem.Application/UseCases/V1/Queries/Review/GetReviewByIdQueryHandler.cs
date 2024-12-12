using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Review;

public sealed class GetReviewByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : IQueryHandler<Query.GetReviewByIdQuery, Response.ReviewResponse>
{
    public async Task<Result<Response.ReviewResponse>> Handle
        (Query.GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Review review = await reviewRepository.FindByIdAsync(request.Id, cancellationToken)
                                        ?? throw new ReviewException.ReviewNotFoundException(request.Id);

        Response.ReviewResponse? result = mapper.Map<Response.ReviewResponse>(review);

        return Result.Success(result);
    }
}
