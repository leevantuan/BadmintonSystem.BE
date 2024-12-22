namespace BadmintonSystem.Domain.Exceptions;

public static class YardPriceException
{
    public class YardPriceNotFoundException : NotFoundException
    {
        public YardPriceNotFoundException(Guid yardPriceId)
            : base($"The yard price with the id {yardPriceId} was not found.")
        {
        }
    }
}
