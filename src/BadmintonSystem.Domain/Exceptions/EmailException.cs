namespace BadmintonSystem.Domain.Exceptions;

public static class EmailException
{
    public class EmailNotFoundException : NotFoundException
    {
        public EmailNotFoundException()
            : base("The email was not found.")
        {
        }
    }
}
