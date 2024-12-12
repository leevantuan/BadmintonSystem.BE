namespace BadmintonSystem.Contract.Services.V1.Category;
public static class Request
{
    public record CreateCategoryRequest(string Name);

    public class UpdateCategoryRequest
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }
    }
}
