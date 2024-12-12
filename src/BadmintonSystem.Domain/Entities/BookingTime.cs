namespace BadmintonSystem.Domain.Entities;

public class BookingTime
{
    public Guid TimeSlotId { get; set; }

    public Guid BookingLineId { get; set; }
}
