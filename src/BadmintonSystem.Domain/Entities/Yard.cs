using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Yard : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public Guid YardTypeId { get; set; }

    public virtual ICollection<BookingLine>? BookingLines { get; set; }
}
