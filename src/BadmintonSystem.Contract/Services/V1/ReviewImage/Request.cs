namespace BadmintonSystem.Contract.Services.V1.ReviewImage;

public static class Request
{
    public record CreateReviewImageRequest(string ImageLink, Guid ReviewId);

    public class UpdateReviewImageRequest
    {
        public Guid Id { get; set; }

        public string? ImageLink { get; set; }

        public Guid ReviewId { get; set; }
    }
}
