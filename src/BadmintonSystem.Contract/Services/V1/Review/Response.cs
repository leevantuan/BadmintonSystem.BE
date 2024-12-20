using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Review;

public static class Response
{
    public record ReviewResponse(Guid Id, string Comment, int RatingValue, Guid UserId, Guid ClubId);

    public class ReviewDetailResponse : EntityAuditBase<Guid>
    {
        public string? Comment { get; set; }

        public int RatingValue { get; set; }

        public Guid UserId { get; set; }

        public Guid ClubId { get; set; }
    }
}
