namespace BadmintonSystem.Domain.Exceptions;
public abstract class BadRequestException : DomainException
{
    // After call to BadRequestException ==> Return "Bad Requuest + Message"
    protected BadRequestException(string message)
        : base("Bad Request", message)
    {
    }
}
