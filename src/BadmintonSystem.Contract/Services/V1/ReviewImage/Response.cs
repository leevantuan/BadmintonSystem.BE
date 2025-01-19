namespace BadmintonSystem.Contract.Services.V1.ReviewImage;

public static class Response
{
    public class ReviewImageDetailResponse
    {
        public Guid Id { get; set; }

        public string? ImageLink { get; set; }

        public Guid ReviewId { get; set; }
    }
}
