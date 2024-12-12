using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class BookingLine : EntityAuditBase<Guid>
{
    public decimal TotalPrice { get; set; }

    public Guid BookingId { get; set; }

    public Guid YardId { get; set; }

    public virtual ICollection<BookingTime>? BookingTimes { get; set; }
}
