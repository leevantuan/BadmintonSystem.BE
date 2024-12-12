using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Sale : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public int Percent { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ActiveEnum IsActive { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; }
}
