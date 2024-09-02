namespace BadmintonSystem.Domain.Exceptions;
public abstract class DomainException : Exception
{
    // This is contructor ==> It takes 2 parameters
    // Base(massage) call to parent "Exception" send to message
    // Create Title
    protected DomainException(string title, string message)
        : base(message) =>
        Title = title;

    public string Title { get; }
}
