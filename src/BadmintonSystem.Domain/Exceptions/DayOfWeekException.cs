namespace BadmintonSystem.Domain.Exceptions;

public static class DayOfWeekException
{
    public class DayOfWeekNotFoundException : NotFoundException
    {
        public DayOfWeekNotFoundException(Guid dayOfWeekId)
            : base($"The Day Of Week with the id {dayOfWeekId} was not found.")
        {
        }
    }
}
