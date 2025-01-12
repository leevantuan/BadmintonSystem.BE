using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Response
{
    public record BookingResponse(
        Guid Id,
        DateTime BookingDate,
        decimal BookingTotal,
        int PaymentStatus,
        int BookingStatus);

    public class BookingDetailResponse : EntityAuditBase<Guid>
    {
        public DateTime? BookingDate { get; set; }

        public decimal? BookingTotal { get; set; }

        public Guid? UserId { get; set; }

        public Guid? SaleId { get; set; }

        public int? BookingStatus { get; set; }

        public int? PaymentStatus { get; set; }
    }
}
