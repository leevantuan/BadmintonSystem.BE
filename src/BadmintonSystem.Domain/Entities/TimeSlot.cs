﻿using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class TimeSlot : EntityAuditBase<Guid>
{
    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual ICollection<YardPrice>? YardPrices { get; set; }

    public virtual ICollection<TimeSlotOfWeek>? TimeSlotOfWeeks { get; set; }
}
