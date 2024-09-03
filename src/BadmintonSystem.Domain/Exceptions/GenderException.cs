namespace BadmintonSystem.Domain.Exceptions;
public class GenderException
{
    // If can't find genderId then call it
    public class GenderNotFoundException : NotFoundException
    {
        public GenderNotFoundException(Guid genderId)
            : base($"===========> The gender with the id {genderId} was not found.") { }
    }

    // If can't find genderId then call it
    public class GenderBadRequestException : BadRequestException
    {
        public GenderBadRequestException(string message)
            : base($"===========> Bad Request: {message}.")
        {
        }
    }
}
