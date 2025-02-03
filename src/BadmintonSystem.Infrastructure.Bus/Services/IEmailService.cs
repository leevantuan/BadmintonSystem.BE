using BadmintonSystem.Contract.Abstractions.IntegrationEvents;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public interface IEmailService
{
    Task SendEmailAsync(BusCommand.SendEmailBusCommand request, CancellationToken cancellationToken);
}
