using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Review;

public sealed class CreateReviewCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Review, Guid> reviewRepository)
    : ICommandHandler<Command.CreateReviewCommand, Response.ReviewResponse>
{
    public Task<Result<Response.ReviewResponse>> Handle
        (Command.CreateReviewCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Review review = mapper.Map<Domain.Entities.Review>(request.Data);

        reviewRepository.Add(review);

        Response.ReviewResponse? result = mapper.Map<Response.ReviewResponse>(review);

        return Task.FromResult(Result.Success(result));
    }
}
