using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class InventoryReceipt : EntityAuditBase<Guid>
{
    public decimal? Quantity { get; set; }

    public string? Unit { get; set; }

    public decimal Price { get; set; }

    public Guid? ServiceId { get; set; }

    public Guid ProviderId { get; set; }
}
