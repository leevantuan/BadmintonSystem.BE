using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Review;

public sealed class UpdateReviewCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : ICommandHandler<Command.UpdateReviewCommand, Response.ReviewResponse>
{
    public async Task<Result<Response.ReviewResponse>> Handle
        (Command.UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Review review = await reviewRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                        ?? throw new ReviewException.ReviewNotFoundException(request.Data.Id);

        review.Comment = request.Data.Comment ?? review.Comment;
        review.RatingValue = request.Data.RatingValue;
        review.UserId = request.Data.UserId;

        Response.ReviewResponse? result = mapper.Map<Response.ReviewResponse>(review);

        return Result.Success(result);
    }
}
