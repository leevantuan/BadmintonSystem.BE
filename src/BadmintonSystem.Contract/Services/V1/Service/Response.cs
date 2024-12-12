using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Response
{
    public record ServiceResponse(
        Guid Id,
        string Name,
        decimal SellingPrice,
        decimal PurchasePrice,
        Guid CategoryId,
        Guid ClubId);

    public class ServiceDetailResponse : EntityAuditBase<Guid>
    {
        public string Name { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal PurchasePrice { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ClubId { get; set; }
    }
}
