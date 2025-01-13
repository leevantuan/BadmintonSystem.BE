namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IBillService
{
    Task DeleteBillByBookingId(Guid bookingId, CancellationToken cancellationToken);
}
