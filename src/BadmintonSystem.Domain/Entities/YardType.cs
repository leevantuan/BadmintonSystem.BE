using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class YardType : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public Guid PriceId { get; set; }

    public virtual ICollection<Yard>? Yards { get; set; }
}
