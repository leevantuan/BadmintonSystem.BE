using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Service : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal QuantityInStock { get; set; }

    public string? Unit { get; set; }

    public Guid CategoryId { get; set; }

    public virtual ICollection<ServiceLine>? ServiceLines { get; set; }

    public virtual ICollection<InventoryReceipt>? InventoryReceipts { get; set; }
}
