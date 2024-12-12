namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Request
{
    public record CreateYardRequest(string Name, Guid YardTypeId);

    public class UpdateYardRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid YardTypeId { get; set; }
    }
}
