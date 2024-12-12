namespace BadmintonSystem.Domain.Exceptions;

public static class YardTypeException
{
    public class YardTypeNotFoundException : NotFoundException
    {
        public YardTypeNotFoundException(Guid yardTypeId)
            : base($"The yard type with the id {yardTypeId} was not found.")
        {
        }
    }
}
