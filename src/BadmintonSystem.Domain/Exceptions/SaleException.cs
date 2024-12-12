namespace BadmintonSystem.Domain.Exceptions;

public static class SaleException
{
    public class SaleNotFoundException : NotFoundException
    {
        public SaleNotFoundException(Guid saleId)
            : base($"The sale with the id {saleId} was not found.")
        {
        }
    }
}
