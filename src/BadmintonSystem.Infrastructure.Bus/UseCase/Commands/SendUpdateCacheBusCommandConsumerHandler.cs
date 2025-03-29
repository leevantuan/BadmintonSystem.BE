using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Infrastructure.Bus.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Commands;

public sealed class SendUpdateCacheBusCommandConsumerHandler(
    ICachingService cachingService,
    ILogger<BusCommand.SendUpdateCacheBusCommand> logger)
    : IRequestHandler<BusCommand.SendUpdateCacheBusCommand>
{
    public async Task Handle(BusCommand.SendUpdateCacheBusCommand request, CancellationToken cancellationToken)
    {
        await cachingService.SendUpdateCachingAsync(request, cancellationToken);
    }
}
