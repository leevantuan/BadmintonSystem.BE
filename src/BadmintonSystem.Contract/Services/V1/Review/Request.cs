namespace BadmintonSystem.Contract.Services.V1.Review;

public static class Request
{
    public class CreateReviewRequest
    {
        public string? Comment { get; set; }

        public int RatingValue { get; set; }

        public Guid UserId { get; set; }

        public Guid ClubId { get; set; }
    }

    public class UpdateReviewRequest
    {
        public Guid Id { get; set; }

        public string? Comment { get; set; }

        public int RatingValue { get; set; }

        public Guid UserId { get; set; }

        public Guid ClubId { get; set; }
    }

    // REVIEW REQUEST BY USER

    public class CreateReviewByUserIdRequest : CreateReviewRequest
    {
        public List<ReviewImage.Request.CreateReviewImageRequest>? ReviewImages { get; set; }
    }

    public class UpdateReviewByUserIdRequest : CreateReviewRequest
    {
        public Guid Id { get; set; }

        public List<ReviewImage.Request.CreateReviewImageRequest>? ReviewImages { get; set; }
    }
}
