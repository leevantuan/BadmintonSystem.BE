using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class ComboFixed : EntityAuditBase<Guid>
{
    public decimal Price { get; set; }

    public string? Content { get; set; }

    public virtual ICollection<ServiceLine>? ServiceLines { get; set; }
}
