namespace BadmintonSystem.Contract.Services.V1.ReviewImage;

public static class Request
{
    public record CreateReviewImageRequest(string ImageLink, Guid ReviewId);
}
