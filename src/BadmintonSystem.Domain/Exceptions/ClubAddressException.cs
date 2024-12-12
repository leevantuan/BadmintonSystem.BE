namespace BadmintonSystem.Domain.Exceptions;
public static class ClubAddressException
{
    public class ClubAddressNotFoundException : NotFoundException
    {
        public ClubAddressNotFoundException(Guid clubAddressId)
            : base($"The clubAddress with the id {clubAddressId} was not found.")
        { }
    }
}
