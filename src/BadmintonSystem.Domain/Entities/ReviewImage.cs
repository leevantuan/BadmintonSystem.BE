using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class ReviewImage : EntityAuditBase<Guid>
{
    public string ImageLink { get; set; }

    public Guid ReviewId { get; set; }
}
