using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.BookingLine;

public static class Response
{
    public record BookingLineResponse(
        Guid Id,
        decimal TotalPrice,
        Guid YardId,
        Guid BookingId);

    public class BookingLineDetailResponse : EntityAuditBase<Guid>
    {
        public decimal? TotalPrice { get; set; }

        public Guid? YardId { get; set; }

        public Guid? BookingId { get; set; }
    }
}
