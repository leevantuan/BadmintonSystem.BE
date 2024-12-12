using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Address : EntityAuditBase<Guid>
{
    public string? Unit { get; set; }

    public string? Street { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public virtual ICollection<UserAddress>? UserAddresses { get; set; }

    public virtual ICollection<ClubAddress> ClubAddresses { get; set; }
}
