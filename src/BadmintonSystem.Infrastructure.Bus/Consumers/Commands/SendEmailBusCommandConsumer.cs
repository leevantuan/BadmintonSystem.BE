using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.Consumers.Commands;

public class SendEmailBusCommandConsumer(ISender sender)
    : ConsumerCommand<BusCommand.SendEmailBusCommand>(sender)
{
}
