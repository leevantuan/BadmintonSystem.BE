namespace BadmintonSystem.Domain.Exceptions;
public static class AddressException
{
    public class AddressNotFoundException : NotFoundException
    {
        public AddressNotFoundException(Guid addressId)
            : base($"The address with the id {addressId} was not found.")
        { }
    }
}
