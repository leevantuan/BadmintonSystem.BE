using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Booking : EntityAuditBase<Guid>
{
    public DateTime BookingDate { get; set; }

    public decimal BookingTotal { get; set; }

    public decimal OriginalPrice { get; set; }

    public BookingStatusEnum BookingStatus { get; set; }

    public PaymentStatusEnum PaymentStatus { get; set; }

    public Guid? UserId { get; set; }

    public Guid? SaleId { get; set; }

    public int? PercentPrePay { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<BookingLine> BookingLines { get; set; }
}
