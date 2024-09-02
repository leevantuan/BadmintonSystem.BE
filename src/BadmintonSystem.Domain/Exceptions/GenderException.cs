namespace BadmintonSystem.Domain.Exceptions;
public class GenderException
{
    // If can't find genderId then call it
    public class GenderNotFoundException : NotFoundException
    {
        public GenderNotFoundException(Guid genderId)
            : base($"The product with the id {genderId} was not found.") { }
    }
}
