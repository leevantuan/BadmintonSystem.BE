using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class PaymentMethod : EntityAuditBase<Guid>
{
    public string Provider { get; set; }

    public string? AccountNumber { get; set; }

    public DateTime? Expiry { get; set; }

    public DefaultEnum IsDefault { get; set; }

    // public Guid PaymentTypeId { get; set; }

    public Guid UserId { get; set; }
}
