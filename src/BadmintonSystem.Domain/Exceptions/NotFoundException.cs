namespace BadmintonSystem.Domain.Exceptions;
public abstract class NotFoundException : DomainException
{
    // After call to NotFoundException ==> Return "Not Found + Message"
    protected NotFoundException(string message)
        : base("Not Found", message)
    {
    }
}
