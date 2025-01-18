using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class BillLine : EntityAuditBase<Guid>
{
    public Guid BillId { get; set; }

    public Guid? YardId { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public decimal? TotalPrice { get; set; }

    public ActiveEnum? IsActive { get; set; }
}
