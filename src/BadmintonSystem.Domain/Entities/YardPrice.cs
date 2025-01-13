using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class YardPrice : EntityAuditBase<Guid>
{
    public Guid YardId { get; set; }

    public Guid? PriceId { get; set; }

    public Guid TimeSlotId { get; set; }

    public DateTime EffectiveDate { get; set; }

    public BookingEnum IsBooking { get; set; }

    public virtual ICollection<BookingLine>? BookingLines { get; set; }
}
