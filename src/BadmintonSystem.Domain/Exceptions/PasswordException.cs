namespace BadmintonSystem.Domain.Exceptions;

public static class PasswordException
{
    public class PasswordNotConfirmException : NotConfirmException
    {
        public PasswordNotConfirmException()
            : base("The password was not confirm.")
        {
        }
    }

    public class PasswordNotMatchException : NotConfirmException
    {
        public PasswordNotMatchException()
            : base("The password was not match.")
        {
        }
    }
}
