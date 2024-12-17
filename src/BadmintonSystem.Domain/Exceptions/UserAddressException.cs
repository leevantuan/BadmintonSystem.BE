namespace BadmintonSystem.Domain.Exceptions;

public static class UserAddressException
{
    public class UserAddressNotFoundException : NotFoundException
    {
        public UserAddressNotFoundException()
            : base("The user is not address.")
        {
        }
    }
}
