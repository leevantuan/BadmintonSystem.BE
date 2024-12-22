using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.YardType;

public static class Response
{
    public record YardTypeResponse(Guid Id, string Name, Guid PriceId);

    public class YardTypeDetailResponse : EntityBase<Guid>
    {
        public string? Name { get; set; }

        public Guid PriceId { get; set; }
    }
}
