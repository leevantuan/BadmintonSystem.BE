namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Request
{
    public class CreateInventoryReceiptRequest
    {
        public decimal Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid ProviderId { get; set; }
    }

    public class FilterInventoryReceiptRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<Guid>? ServiceIds { get; set; }

        public List<Guid>? ProviderIds { get; set; }
    }
}
