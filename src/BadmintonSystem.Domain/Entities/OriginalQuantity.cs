using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class OriginalQuantity : EntityAuditBase<Guid>
{
    public decimal? TotalQuantity { get; set; }

    public virtual ICollection<Service>? Services { get; set; }
}
