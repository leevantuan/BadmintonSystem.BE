namespace BadmintonSystem.Domain.Exceptions;

public static class ReviewImageException
{
    public class ReviewImageNotFoundException : NotFoundException
    {
        public ReviewImageNotFoundException(Guid reviewImageId)
            : base($"The reviewImage with the id {reviewImageId} was not found.")
        {
        }
    }
}
