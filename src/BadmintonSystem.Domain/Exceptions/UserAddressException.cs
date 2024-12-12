namespace BadmintonSystem.Domain.Exceptions;
public static class UserAddressException
{
    public class UserAddressNotFoundException : NotFoundException
    {
        public UserAddressNotFoundException(Guid userAddressId)
            : base($"The userAddress with the id {userAddressId} was not found.")
        { }
    }
}
