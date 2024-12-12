using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Review;

public sealed class DeleteReviewsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : ICommandHandler<Command.DeleteReviewsCommand>
{
    public async Task<Result> Handle(Command.DeleteReviewsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Review> reviews = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Review review = await reviewRepository.FindByIdAsync(idValue, cancellationToken)
                                            ?? throw new ReviewException.ReviewNotFoundException(idValue);

            reviews.Add(review);
        }

        reviewRepository.RemoveMultiple(reviews);

        return Result.Success();
    }
}
