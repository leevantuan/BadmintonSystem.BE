using static BadmintonSystem.Contract.Abstractions.IntegrationEvents.BusCommand;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IBookingService
{
    Task SignalRAndUpdateCacheAsync(SendUpdateCacheBusCommand request, CancellationToken cancellationToken);
}
