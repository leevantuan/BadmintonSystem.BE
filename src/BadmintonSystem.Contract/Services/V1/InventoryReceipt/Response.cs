using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Response
{
    public class InventoryReceiptResponse : EntityBase<Guid>
    {
        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal? Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid? ProviderId { get; set; }
    }

    public class InventoryReceiptDetail : EntityAuditBase<Guid>
    {
        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal? Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid? ProviderId { get; set; }
    }

    public class InventoryReceiptDetailResponse : EntityAuditBase<Guid>
    {
        public InventoryReceiptResponse? InventoryReceipt { get; set; }

        public Service.Response.ServiceResponse? Service { get; set; }

        public decimal? TotalPrice { get; set; }
    }

    public class InventoryReceiptDetailSql : Provider.Response.GetInventoryReceiptByProviderIdSql
    {
        public DateTime? InventoryReceipt_CreatedDate { get; set; }

        public DateTime? InventoryReceipt_ModifiedDate { get; set; }

        public Guid? InventoryReceipt_CreatedBy { get; set; }

        public Guid? InventoryReceipt_ModifiedBy { get; set; }

        public bool? InventoryReceipt_IsDeleted { get; set; }

        public DateTime? InventoryReceipt_DeletedAt { get; set; }
    }
}
