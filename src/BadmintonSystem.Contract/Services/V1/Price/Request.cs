namespace BadmintonSystem.Contract.Services.V1.Price;

public static class Request
{
    public record CreatePriceRequest(
        decimal YardPrice,
        int IsDefault);

    public class UpdatePriceRequest
    {
        public Guid Id { get; set; }

        public decimal YardPrice { get; set; }

        public int IsDefault { get; set; }
    }
}
