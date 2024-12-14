namespace BadmintonSystem.Domain.Exceptions;

public abstract class NotConfirmException : DomainException
{
    protected NotConfirmException(string message)
        : base("Not Confirm", message)
    {
    }
}
