using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.YardType;

public static class Response
{
    public record YardTypeResponse(Guid Id, string Name, decimal Price);

    public class YardTypeDetailResponse : EntityAuditBase<Guid>
    {
        public string? Name { get; set; }

        public decimal Price { get; set; }
    }
}
