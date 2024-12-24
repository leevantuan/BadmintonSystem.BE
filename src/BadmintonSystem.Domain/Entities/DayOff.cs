using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class DayOff : EntityAuditBase<Guid>
{
    public DateTime Date { get; set; }
}
