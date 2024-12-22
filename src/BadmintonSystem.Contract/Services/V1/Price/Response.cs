using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Price;

public static class Response
{
    public record PriceResponse(
        decimal YardPrice,
        int IsDefault);

    public class PriceDetailResponse : EntityBase<Guid>
    {
        public decimal YardPrice { get; set; }

        public int IsDefault { get; set; }
    }
}
