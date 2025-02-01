using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Commands;

public sealed class SendEmailBusCommandConsumerHandler(
    ILogger<BusCommand.SendEmailBusCommand> logger)
    : IRequestHandler<BusCommand.SendEmailBusCommand>
{
    public async Task Handle(BusCommand.SendEmailBusCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(10000, cancellationToken);
        logger.LogInformation($"Email notification bus command processed: {request.Name}");
    }
}
