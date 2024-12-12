namespace BadmintonSystem.Domain.Exceptions;

public abstract class UnauthorizedException : DomainException
{
    protected UnauthorizedException(string message)
        : base("Unauthorized", message)
    { }
}
