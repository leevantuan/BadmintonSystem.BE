namespace BadmintonSystem.Domain.Exceptions;

public static class TimeSlotException
{
    public class TimeSlotNotFoundException : NotFoundException
    {
        public TimeSlotNotFoundException(Guid timeSlotId)
            : base($"The time slot with the id {timeSlotId} was not found.")
        {
        }
    }
}
