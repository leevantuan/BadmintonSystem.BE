namespace BadmintonSystem.Contract.Services.V1.YardType;

public static class Request
{
    public record CreateYardTypeRequest(string Name, Guid PriceId);

    public class UpdateYardTypeRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid? PriceId { get; set; }
    }
}
