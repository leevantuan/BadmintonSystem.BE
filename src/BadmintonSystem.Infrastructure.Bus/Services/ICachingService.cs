using BadmintonSystem.Contract.Abstractions.IntegrationEvents;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public interface ICachingService
{
    Task SendUpdateCachingAsync
        (BusCommand.SendUpdateCacheBusCommand request, CancellationToken cancellationToken = default);
}
