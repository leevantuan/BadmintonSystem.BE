namespace BadmintonSystem.Domain.Exceptions;

public abstract class ForbiddenException : DomainException
{
    protected ForbiddenException(string message)
        : base("Forbidden", message)
    { }
}
