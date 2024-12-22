namespace BadmintonSystem.Domain.Exceptions;

public static class PriceException
{
    public class PriceNotFoundException : NotFoundException
    {
        public PriceNotFoundException(Guid priceId)
            : base($"The price with the id {priceId} was not found.")
        {
        }
    }
}
