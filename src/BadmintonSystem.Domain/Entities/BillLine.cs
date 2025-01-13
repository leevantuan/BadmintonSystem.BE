using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class BillLine : EntityBase<Guid>
{
    public Guid BillId { get; set; }

    public Guid? YardId { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public decimal? TotalPrice { get; set; }
}
