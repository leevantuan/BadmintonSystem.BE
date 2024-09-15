using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class UserAddress : AuditableEntity<Guid>
{
    public Guid? AddressId { get; private set; }
    public Guid? AppUserId { get; private set; }
    public bool IsDefault { get; private set; }
}
