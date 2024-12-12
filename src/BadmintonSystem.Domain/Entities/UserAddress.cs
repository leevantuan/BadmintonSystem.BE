using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class UserAddress
{
    public Guid AddressId { get; set; }

    public Guid UserId { get; set; }

    public DefaultEnum IsDefault { get; set; }
}
