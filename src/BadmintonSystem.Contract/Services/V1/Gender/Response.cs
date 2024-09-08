namespace BadmintonSystem.Contract.Services.V1.Gender;
public static class Response
{
    public record GenderResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
