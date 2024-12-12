namespace BadmintonSystem.Domain.Exceptions;

public static class ReviewException
{
    public class ReviewNotFoundException : NotFoundException
    {
        public ReviewNotFoundException(Guid reviewId)
            : base($"The review with the id {reviewId} was not found.")
        {
        }
    }
}
