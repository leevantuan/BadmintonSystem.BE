using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Commands;

public sealed class SendEmailBusCommandConsumerHandler : IRequestHandler<BusCommand.SendEmailBusCommand>
{
    public Task Handle(BusCommand.SendEmailBusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
