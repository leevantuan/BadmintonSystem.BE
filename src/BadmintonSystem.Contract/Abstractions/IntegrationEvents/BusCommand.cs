using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Services.V1.Booking;

namespace BadmintonSystem.Contract.Abstractions.IntegrationEvents;

public static class BusCommand
{
    public record SendEmailBusCommand : IBusCommand
    {
        public DateTime SendDate { get; set; }

        public string Type { get; set; }

        public List<Guid> BookingIds { get; set; }

        public string Email { get; set; }

        public Guid Id { get; set; }

        public DateTime TimeSpan { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid TransactionId { get; set; }
    }

    public record SendUpdateCacheBusCommand : IBusCommand
    {
        public List<Response.GetIdsByDate> YardPriceIds { get; set; }

        public Guid Id { get; set; }

        public DateTime TimeSpan { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid TransactionId { get; set; }
    }

    public record SendCreateBookingCommand : IBusCommand
    {
        public Request.CreateBookingRequest CreateBooking { get; set; }

        public Guid UserId { get; set; }

        public Guid Id { get; set; }

        public DateTime TimeSpan { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid TransactionId { get; set; }
    }
}
