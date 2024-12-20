using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Review;

public sealed class DeleteReviewByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : ICommandHandler<Command.DeleteReviewByUserIdCommand>
{
    public async Task<Result> Handle(Command.DeleteReviewByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Domain.Entities.Review review = await reviewRepository.FindByIdAsync(request.ReviewId, cancellationToken)
                                        ?? throw new ReviewException.ReviewNotFoundException(request.ReviewId);

        var reviewImages = context.ReviewImage.Where(x => review.Id == x.ReviewId).ToList();

        reviewRepository.Remove(review);
        context.ReviewImage.RemoveRange(reviewImages);

        return Result.Success();
    }
}
