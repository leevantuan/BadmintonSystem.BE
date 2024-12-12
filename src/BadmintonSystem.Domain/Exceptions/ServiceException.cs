namespace BadmintonSystem.Domain.Exceptions;
public static class ServiceException
{
    public class ServiceNotFoundException : NotFoundException
    {
        public ServiceNotFoundException(Guid serviceId)
            : base($"The service with the id {serviceId} was not found.")
        { }
    }
}
