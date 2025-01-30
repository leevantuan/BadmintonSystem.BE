using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Abstractions.IntegrationEvents;

public static class BusEvent
{
    public record EmailNotificationBusEvent : IBusEvent, ICommand
    {
        public string Type { get; set; }

        public DateTime SendDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid TransactionId { get; set; }

        // Bus Event
        public Guid Id { get; set; }

        public DateTime TimeSpan { get; set; }
    }
}
