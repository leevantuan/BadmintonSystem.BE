using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Response
{
    public class ServiceResponse : EntityBase<Guid>
    {
        public string Name { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal QuantityInStock { get; set; }

        public string? Unit { get; set; }

        public Guid CategoryId { get; set; }

        public decimal? QuantityPrinciple { get; set; }

        public Guid? OriginalQuantityId { get; set; }
    }

    public class ServiceDetailResponse : EntityAuditBase<Guid>
    {
        public string Name { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal QuantityInStock { get; set; }

        public string? Unit { get; set; }

        public Guid CategoryId { get; set; }

        public decimal? QuantityPrinciple { get; set; }

        public Guid? OriginalQuantityId { get; set; }
    }

    public class GetInventoryReceiptByServiceIdResponse
    {
        public ServiceResponse Service { get; set; }

        public List<GetInventoryReceiptDetail>? InventoryReceipts { get; set; }
    }

    public class GetInventoryReceiptDetail
    {
        public InventoryReceipt.Response.InventoryReceiptResponse InventoryReceipt { get; set; }

        public Provider.Response.ProviderResponse Provider { get; set; }
    }

    public class GetInventoryReceiptByServiceIdSql : Provider.Response.InventoryReceiptSql
    {
        // Provider
        public Guid? Provider_Id { get; set; }

        public string? Provider_Name { get; set; }

        public string? Provider_PhoneNumber { get; set; }

        public string? Provider_Address { get; set; }
    }
}
