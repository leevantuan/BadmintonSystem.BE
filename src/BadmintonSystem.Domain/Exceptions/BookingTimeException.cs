namespace BadmintonSystem.Domain.Exceptions;

public static class BookingTimeTypeException
{
    public class BookingTimeTypeNotFoundException : NotFoundException
    {
        public BookingTimeTypeNotFoundException(Guid bookingTimeId)
            : base($"The booking time with the id {bookingTimeId} was not found.")
        {
        }
    }
}
