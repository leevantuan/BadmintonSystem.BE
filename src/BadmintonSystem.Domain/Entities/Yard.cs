using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Yard : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public Guid YardTypeId { get; set; }

    public StatusEnum IsStatus { get; set; }

    public virtual ICollection<YardPrice>? YardPrices { get; set; }

    public virtual ICollection<FixedSchedule>? FixedSchedules { get; set; }
}
