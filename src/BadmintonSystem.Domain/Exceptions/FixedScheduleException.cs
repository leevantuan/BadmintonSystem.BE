namespace BadmintonSystem.Domain.Exceptions;

public static class FixedScheduleException
{
    public class FixedScheduleNotFoundException : NotFoundException
    {
        public FixedScheduleNotFoundException(Guid fixedScheduleId)
            : base($"The Fixed Schedule with the id {fixedScheduleId} was not found.")
        {
        }
    }
}
