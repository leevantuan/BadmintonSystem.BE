namespace BadmintonSystem.Domain.Exceptions;

public static class ProviderException
{
    public class ProviderNotFoundException : NotFoundException
    {
        public ProviderNotFoundException(Guid providerId)
            : base($"The provider with the id {providerId} was not found.")
        {
        }
    }
}
