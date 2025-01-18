namespace BadmintonSystem.Domain.Exceptions;

public static class ServiceLineException
{
    public class ServiceLineNotFoundException : NotFoundException
    {
        public ServiceLineNotFoundException(Guid serviceLineId)
            : base($"The service line with the id {serviceLineId} was not found.")
        {
        }
    }
}
