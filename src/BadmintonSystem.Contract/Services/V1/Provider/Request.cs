namespace BadmintonSystem.Contract.Services.V1.Provider;

public static class Request
{
    public class CreateProviderRequest
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class UpdateProviderRequest
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
