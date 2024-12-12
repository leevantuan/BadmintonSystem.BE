using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.ReviewImage;

public static class Command
{
    public record CreateReviewImageCommand(Guid UserId, Request.CreateReviewImageRequest Data)
        : ICommand;

    public record UpdateReviewImageCommand(Request.UpdateReviewImageRequest Data)
        : ICommand;

    public record DeleteReviewImagesCommand(List<string> Ids)
        : ICommand;
}
