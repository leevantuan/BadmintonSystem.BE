namespace BadmintonSystem.Contract.Services.V1.PaymentMethod;

public static class Request
{
    public record CreatePaymentMethodRequest(
        string Provider,
        int AccountNumber,
        DateTime? Expiry,
        int IsDefault,
        Guid UserId);

    public class UpdatePaymentMethodRequest
    {
        public Guid Id { get; set; }

        public int? AccountNumber { get; set; }

        public DateTime? Expiry { get; set; }

        public string? Provider { get; set; }

        public int? Default { get; set; }

        public Guid? UserId { get; set; }
    }
}
