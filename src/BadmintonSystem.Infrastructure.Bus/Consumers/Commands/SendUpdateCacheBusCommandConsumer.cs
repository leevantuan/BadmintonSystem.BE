using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.Consumers.Commands;

public class SendUpdateCacheBusCommandConsumer(ISender sender)
    : ConsumerCommand<BusCommand.SendUpdateCacheBusCommand>(sender)
{
}
