using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class FixedSchedule : EntityAuditBase<Guid>
{
    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<DayOfWeek>? DaysOfWeeks { get; set; }
}
