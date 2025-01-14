namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Request
{
    public class CreateServiceRequest
    {
        public decimal QuantityInStock { get; set; }

        public Guid CategoryId { get; set; }

        public int IsWholeSale { get; set; }

        public List<ServiceDetail> ServiceDetails { get; set; }
    }

    public class ServiceDetail
    {
        public string Name { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public string? Unit { get; set; }

        public decimal? QuantityPrinciple { get; set; }
    }

    public class UpdateServiceRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public decimal? PurchasePrice { get; set; }

        public decimal? SellingPrice { get; set; }

        public decimal? QuantityInStock { get; set; }

        public string? Unit { get; set; }

        public Guid? CategoryId { get; set; }

        public decimal? QuantityPrinciple { get; set; }

        public Guid? OriginalQuantityId { get; set; }
    }
}
