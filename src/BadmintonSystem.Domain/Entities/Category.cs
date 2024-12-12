using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Category : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public virtual ICollection<Service>? Services { get; set; }
}
