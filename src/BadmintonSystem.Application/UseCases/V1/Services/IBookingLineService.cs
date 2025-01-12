namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IBookingLineService
{
    Task<decimal> CreateBookingLines(Guid bookingId, List<Guid> yardPriceIds, CancellationToken cancellationToken);
}
