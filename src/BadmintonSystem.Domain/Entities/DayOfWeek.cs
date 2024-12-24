using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class DayOfWeek : EntityAuditBase<Guid>
{
    public Guid FixedScheduleId { get; set; }

    public string WeekName { get; set; }

    public virtual ICollection<TimeSlotOfWeek>? TimeSlotOfWeeks { get; set; }
}
