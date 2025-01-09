using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Price : EntityAuditBase<Guid>
{
    public decimal YardPrice { get; set; }

    public string? Detail { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public string? DayOfWeek { get; set; }

    public DefaultEnum IsDefault { get; set; }

    public Guid? YardTypeId { get; set; }

    public virtual ICollection<YardPrice>? YardPrices { get; set; }
}
