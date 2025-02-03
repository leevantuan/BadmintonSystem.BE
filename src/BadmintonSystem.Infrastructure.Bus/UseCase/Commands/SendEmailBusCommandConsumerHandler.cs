using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Infrastructure.Bus.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Commands;

public sealed class SendEmailBusCommandConsumerHandler(
    IEmailService emailService,
    ILogger<BusCommand.SendEmailBusCommand> logger)
    : IRequestHandler<BusCommand.SendEmailBusCommand>
{
    public async Task Handle(BusCommand.SendEmailBusCommand request, CancellationToken cancellationToken)
    {
        await emailService.SendEmailAsync(request, cancellationToken);
    }
}
