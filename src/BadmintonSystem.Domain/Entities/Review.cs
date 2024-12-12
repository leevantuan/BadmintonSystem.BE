using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Review : EntityAuditBase<Guid>
{
    public string? Comment { get; set; }

    public int RatingValue { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<ReviewImage>? ReviewImages { get; set; }
}
