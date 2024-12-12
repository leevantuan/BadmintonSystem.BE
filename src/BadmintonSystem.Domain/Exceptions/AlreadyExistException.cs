namespace BadmintonSystem.Domain.Exceptions;
public abstract class AlreadyExistException : DomainException
{
    protected AlreadyExistException(string message)
        : base("Already Exist", message)
    { }
}
