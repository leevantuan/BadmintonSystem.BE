namespace BadmintonSystem.Domain.Exceptions;

public static class BookingLineTypeException
{
    public class BookingLineTypeNotFoundException : NotFoundException
    {
        public BookingLineTypeNotFoundException(Guid bookingLineId)
            : base($"The booking line with the id {bookingLineId} was not found.")
        {
        }
    }
}
