namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Request
{
    public class CreateInventoryReceiptRequest
    {
        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid ProviderId { get; set; }
    }

    public class UpdateInventoryReceiptRequest
    {
        public Guid? Id { get; set; }

        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid ProviderId { get; set; }
    }
}
