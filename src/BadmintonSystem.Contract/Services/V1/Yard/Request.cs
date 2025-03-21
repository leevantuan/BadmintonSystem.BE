namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Request
{
    public record CreateYardRequest(string Name, string ImageLink, Guid YardTypeId, int IsStatus);

    public class UpdateYardRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? ImageLink { get; set; }

        public Guid YardTypeId { get; set; }

        public int IsStatus { get; set; }
    }
}
