namespace BadmintonSystem.Contract.Services.V1.Review;

public static class Request
{
    public record CreateReviewRequest(string Comment, int RatingValue, Guid UserId);

    public class UpdateReviewRequest
    {
        public Guid Id { get; set; }

        public string? Comment { get; set; }

        public int RatingValue { get; set; }

        public Guid UserId { get; set; }
    }
}
