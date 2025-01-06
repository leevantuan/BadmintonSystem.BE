using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Provider : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public virtual ICollection<InventoryReceipt>? InventoryReceipts { get; set; }
}
