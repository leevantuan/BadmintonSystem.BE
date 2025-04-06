namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IYardPriceService
{
    Task<bool> CreateYardPrice(DateTime date, Guid userId, CancellationToken cancellationToken);

    Task<List<Contract.Services.V1.YardPrice.Response.YardPricesByDateDetailResponse>> GetYardPrices(
        DateTime date, string tenant, Guid userId, CancellationToken cancellationToken);

    Task UpdateYardPricesByBookingId(Guid bookingId, CancellationToken cancellationToken);
}
