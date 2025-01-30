using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Abstractions.IntegrationEvents;

public static class BusCommad
{
    public record SendEmailBusEvent : IBusCommand
    {
        public DateTime SendDate { get; set; }

        public Guid Id { get; set; }

        public DateTime TimeSpan { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid TransactionId { get; set; }
    }
}
