using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Club : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public string? Hotline { get; set; }

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }

    public virtual ICollection<ClubAddress> ClubAddresses { get; set; }

    public virtual ICollection<ClubImage>? ClubImages { get; set; }

    public virtual ClubInformation? ClubInformation { get; set; }
}
