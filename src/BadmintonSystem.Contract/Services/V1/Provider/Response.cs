using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Provider;

public static class Response
{
    public class ProviderResponse : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class ProviderDetailResponse : EntityAuditBase<Guid>
    {
        public string Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class GetInventoryReceiptByProviderIdResponse
    {
        public ProviderResponse Provider { get; set; }

        public List<GetInventoryReceiptResponse>? InventoryReceipts { get; set; }
    }

    public class GetInventoryReceiptResponse : InventoryReceipt.Response.InventoryReceiptDetailResponse
    {
    }

    public class InventoryReceiptSql
    {
        // Inventory Receipt
        public Guid? InventoryReceipt_Id { get; set; }

        public decimal? InventoryReceipt_Quantity { get; set; }

        public string? InventoryReceipt_Unit { get; set; }

        public decimal? InventoryReceipt_Price { get; set; }

        public Guid? InventoryReceipt_ServiceId { get; set; }

        public Guid? InventoryReceipt_ProviderId { get; set; }
    }

    public class GetInventoryReceiptByProviderIdSql : InventoryReceiptSql
    {
        // Service
        public Guid? Service_Id { get; set; }

        public string? Service_Name { get; set; }

        public decimal? Service_PurchasePrice { get; set; }

        public decimal? Service_SellingPrice { get; set; }

        public decimal? Service_QuantityInStock { get; set; }

        public string? Service_Unit { get; set; }

        public Guid? Service_CategoryId { get; set; }

        public decimal? Service_QuantityPrinciple { get; set; }

        public Guid? Service_OriginalQuantityId { get; set; }
    }
}
