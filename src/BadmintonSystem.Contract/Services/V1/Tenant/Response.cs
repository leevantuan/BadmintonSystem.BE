namespace BadmintonSystem.Contract.Services.V1.Tenant;

public class Response
{
    public class TenantResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? HotLine { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? Slogan { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public string? ConnectionString { get; set; }
    }
}
