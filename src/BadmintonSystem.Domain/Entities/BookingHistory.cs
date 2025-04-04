using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class BookingHistory : EntityBase<Guid>
{
    public Guid BookingId { get; set; }

    public string ClubName { get; set; }

    public TimeSpan StartTime { get; set; }

    public DateTime PlayDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal TotalPrice { get; set; }

    public PaymentStatusEnum PaymentStatus { get; set; }

    public string TenantCode { get; set; }

    public Guid? UserId { get; set; }
}
