namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Request
{
    public record CreateServiceRequest(
        string Name,
        decimal SellingPrice,
        decimal PurchasePrice,
        Guid CategoryId,
        Guid ClubId);

    public class UpdateServiceRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public decimal? SellingPrice { get; set; }

        public decimal? PurchasePrice { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? ClubId { get; set; }
    }
}
