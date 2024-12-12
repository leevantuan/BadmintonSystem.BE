namespace BadmintonSystem.Domain.Exceptions;

public static class BookingException
{
    public class BookingNotFoundException : NotFoundException
    {
        public BookingNotFoundException(Guid bookingId)
            : base($"The booking with the id {bookingId} was not found.")
        {
        }
    }
}
