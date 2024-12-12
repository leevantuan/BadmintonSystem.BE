using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class ClubInformation : EntityAuditBase<Guid>
{
    public string? FacebookPageLink { get; set; }

    public string? InstagramLink { get; set; }

    public string? MapLink { get; set; }

    public Guid ClubId { get; set; }
}
