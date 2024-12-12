namespace BadmintonSystem.Domain.Exceptions;
public static class GenderException
{
    public class GenderNotFoundException : NotFoundException
    {
        public GenderNotFoundException(Guid genderId)
            : base($"The gender with the id {genderId} was not found.")
        { }
    }
}
