using BadmintonSystem.Contract.Abstractions.IntegrationEvents;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public interface IBookingService
{
    Task CreateBookingAsync(BusCommand.SendCreateBookingCommand data, CancellationToken cancellationToken);
}
