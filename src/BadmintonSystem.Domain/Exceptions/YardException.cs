namespace BadmintonSystem.Domain.Exceptions;

public static class YardException
{
    public class YardNotFoundException : NotFoundException
    {
        public YardNotFoundException(Guid yardId)
            : base($"The yard with the id {yardId} was not found.")
        {
        }
    }
}
