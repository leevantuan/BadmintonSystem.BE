namespace BadmintonSystem.Domain.Exceptions;

public static class ClubImageException
{
    public class ClubImageNotFoundException : NotFoundException
    {
        public ClubImageNotFoundException(Guid clubImageId)
            : base($"The club image with the id {clubImageId} was not found.")
        {
        }
    }
}
