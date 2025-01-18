namespace BadmintonSystem.Domain.Exceptions;

public static class BillException
{
    public class BillNotFoundException : NotFoundException
    {
        public BillNotFoundException(Guid billId)
            : base($"The bill with the id {billId} was not found.")
        {
        }
    }
}
