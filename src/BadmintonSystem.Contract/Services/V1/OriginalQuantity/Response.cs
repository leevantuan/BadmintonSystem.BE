using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.OriginalQuantity;

public static class Response
{
    public class OriginalQuantityDetailResponse : EntityAuditBase<Guid>
    {
        public decimal? TotalQuantity { get; set; }
    }
}
