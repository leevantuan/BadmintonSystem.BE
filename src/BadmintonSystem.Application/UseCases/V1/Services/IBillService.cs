using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IBillService
{
    Task DeleteBillByBookingId(Guid bookingId, CancellationToken cancellationToken);

    Task UpdateTotalPriceByBillId(Guid billId, CancellationToken cancellationToken);

    Task ChangeYardActiveByBookingId(Guid billId, StatusEnum status, CancellationToken cancellationToken);
}
