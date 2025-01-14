using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Response
{
    public class InventoryReceiptDetailResponse : EntityAuditBase<Guid>
    {
        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal? Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid? ProviderId { get; set; }
    }
}
