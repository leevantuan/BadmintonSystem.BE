namespace BadmintonSystem.Domain.Exceptions;
public static class CategoryException
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(Guid categoryId)
            : base($"The category with the id {categoryId} was not found.")
        { }
    }
}
