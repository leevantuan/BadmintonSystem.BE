namespace BadmintonSystem.Contract.Services.V2.Category;
public static class Response
{
    public record CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
