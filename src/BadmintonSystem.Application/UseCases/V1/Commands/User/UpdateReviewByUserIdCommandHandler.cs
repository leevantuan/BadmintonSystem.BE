using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.User;

public sealed class UpdateReviewByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : ICommandHandler<Command.UpdateReviewByUserIdCommand>
{
    public async Task<Result> Handle(Command.UpdateReviewByUserIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Review review = await reviewRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                        ?? throw new ReviewException.ReviewNotFoundException(request.Data.Id);

        var reviewImages = context.ReviewImage.Where(i => i.ReviewId == request.Data.Id).ToList();

        IEnumerable<ReviewImage> reviewImagesRequest =
            request.Data.ReviewImages.Select(x => mapper.Map<ReviewImage>(x));

        review.Comment = request.Data.Comment;
        review.RatingValue = request.Data.RatingValue;

        if (reviewImages.Any())
        {
            context.ReviewImage.RemoveRange(reviewImages);

            await context.SaveChangesAsync(cancellationToken);
        }

        foreach (ReviewImage? reviewImage in reviewImagesRequest)
        {
            reviewImage.ReviewId = request.Data.Id;
            context.ReviewImage.Add(reviewImage);
        }

        return Result.Success();
    }
}
