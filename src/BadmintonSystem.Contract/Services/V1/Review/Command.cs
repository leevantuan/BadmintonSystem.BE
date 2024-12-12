using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Review;

public static class Command
{
    public record CreateReviewCommand(Guid UserId, Request.CreateReviewRequest Data)
        : ICommand<Response.ReviewResponse>;

    public record UpdateReviewCommand(Request.UpdateReviewRequest Data)
        : ICommand<Response.ReviewResponse>;

    public record DeleteReviewsCommand(List<string> Ids)
        : ICommand;
}
