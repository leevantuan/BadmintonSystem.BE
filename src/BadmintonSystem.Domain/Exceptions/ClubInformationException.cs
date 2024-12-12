namespace BadmintonSystem.Domain.Exceptions;

public static class ClubInformationException
{
    public class ClubInformationNotFoundException : NotFoundException
    {
        public ClubInformationNotFoundException(Guid clubInformationId)
            : base($"The club information with the id {clubInformationId} was not found.")
        {
        }
    }
}
