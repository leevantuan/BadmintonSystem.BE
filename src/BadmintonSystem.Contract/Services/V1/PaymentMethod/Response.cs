namespace BadmintonSystem.Contract.Services.V1.PaymentMethod;

public static class Response
{
    public record PaymentMethodResponse(
        Guid Id,
        string Provider,
        int AccountNumber,
        DateTime? Expiry,
        int IsDefault,
        Guid UserId);

    public class PaymentMethodDetailResponse
    {
        public Guid Id { get; set; }

        public int? AccountNumber { get; set; }

        public DateTime? Expiry { get; set; }

        public string? Provider { get; set; }

        public int IsDefault { get; set; }

        public Guid? UserId { get; set; }
    }
}
