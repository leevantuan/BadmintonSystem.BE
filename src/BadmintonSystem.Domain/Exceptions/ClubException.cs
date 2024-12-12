namespace BadmintonSystem.Domain.Exceptions;
public static class ClubException
{
    public class ClubNotFoundException : NotFoundException
    {
        public ClubNotFoundException(Guid clubId)
            : base($"The club with the id {clubId} was not found.")
        { }
    }
}
