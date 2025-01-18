namespace BadmintonSystem.Domain.Exceptions;

public static class BillLineException
{
    public class BillLineNotFoundException : NotFoundException
    {
        public BillLineNotFoundException(Guid billLineId)
            : base($"The bill line with the id {billLineId} was not found.")
        {
        }
    }
}
