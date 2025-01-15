using BadmintonSystem.Contract.Abstractions.Entities;
using static BadmintonSystem.Contract.Services.V1.Service.Response;

namespace BadmintonSystem.Contract.Services.V1.Category;

public static class Response
{
    public record CategoryResponse(Guid Id, string Name);

    public class CategoryDetailResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ServiceResponse>? Services { get; set; }
    }

    public class GetServicesByCategoryIdResponse : EntityAuditBase<Guid>
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public virtual List<ServiceDetailResponse> Services { get; set; }
    }

    public class CategorySqlResponse
    {
        public Guid Category_Id { get; set; }

        public string Category_Name { get; set; }

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
