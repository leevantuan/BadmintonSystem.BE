using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class ServiceLine : EntityAuditBase<Guid>
{
    public Guid? ServiceId { get; set; }

    public Guid? ComboFixedId { get; set; }

    public int Quantity { get; set; }

    public Guid BillId { get; set; }
}
