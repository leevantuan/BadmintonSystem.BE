using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Review;

public static class Command
{
    public record CreateReviewByUserIdCommand(Guid UserId, Request.CreateReviewByUserIdRequest Data) : ICommand;

    public record UpdateReviewByUserIdCommand(Guid UserId, Request.UpdateReviewByUserIdRequest Data) : ICommand;

    public record DeleteReviewByUserIdCommand(Guid UserId, Guid ReviewId) : ICommand;
}
