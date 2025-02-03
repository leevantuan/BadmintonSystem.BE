using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Infrastructure.Bus.Services;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Commands;

public sealed class SendCreateBookingCommandConsumerHandler(
    IBookingService bookingService)
    : IRequestHandler<BusCommand.SendCreateBookingCommand>
{
    public async Task Handle(BusCommand.SendCreateBookingCommand request, CancellationToken cancellationToken)
    {
        await bookingService.CreateBookingAsync(request, cancellationToken);
    }
}
