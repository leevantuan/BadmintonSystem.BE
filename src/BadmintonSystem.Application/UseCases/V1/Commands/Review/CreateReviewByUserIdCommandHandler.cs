using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Review;

public sealed class CreateReviewByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper)
    : ICommandHandler<Command.CreateReviewByUserIdCommand>
{
    public async Task<Result> Handle(Command.CreateReviewByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = await context.AppUsers.FirstOrDefaultAsync(x => x.Id == request.Data.UserId, cancellationToken)
            ?? throw new IdentityException.AppUserNotFoundException(request.Data.UserId);

        Domain.Entities.Club? club = await context.Club.FirstOrDefaultAsync(x => x.Id == request.Data.ClubId, cancellationToken);

        Domain.Entities.Review? review = mapper.Map<Domain.Entities.Review>(request.Data);

        IEnumerable<ReviewImage> reviewImages =
            request.Data.ReviewImages.Select(x => mapper.Map<ReviewImage>(x));

        review.Id = Guid.NewGuid();
        review.UserId = request.Data.UserId;
        review.ClubId = club?.Id ?? request.Data.ClubId;

        context.Review.Add(review);

        await context.SaveChangesAsync(cancellationToken);

        foreach (ReviewImage reviewImage in reviewImages)
        {
            reviewImage.ReviewId = review.Id;

            context.ReviewImage.Add(reviewImage);
        }

        return Result.Success();
    }
}
