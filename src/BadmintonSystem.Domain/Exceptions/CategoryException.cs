namespace BadmintonSystem.Domain.Exceptions;

public class CategoryException
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(Guid CategoryId)
            : base($"===========> The Category with the id {CategoryId} was not found.") { }
    }

    public class CategoryBadRequestException : BadRequestException
    {
        public CategoryBadRequestException(string message)
            : base($"===========> Bad Request: {message}.")
        {
        }
    }
}
