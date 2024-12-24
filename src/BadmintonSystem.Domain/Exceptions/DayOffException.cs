namespace BadmintonSystem.Domain.Exceptions;

public static class DayOffException
{
    public class DayOffNotFoundException : NotFoundException
    {
        public DayOffNotFoundException(Guid dayOffId)
            : base($"The Day Off with the id {dayOffId} was not found.")
        {
        }
    }
}
