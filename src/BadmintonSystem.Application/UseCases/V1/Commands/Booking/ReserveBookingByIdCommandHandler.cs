using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using MassTransit;
using Response = BadmintonSystem.Contract.Services.V1.Booking.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class ReserveBookingByIdCommandHandler(
    IBookingHub bookingHub,
    IBus bus,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.ReserveBookingByIdCommand>
{
    public async Task<Result> Handle(Command.ReserveBookingByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardPrice yardPrice = await yardPriceRepository.FindByIdAsync(request.Id, cancellationToken)
                                              ?? throw new YardPriceException.YardPriceNotFoundException(request.Id);

        var ids = new List<Guid> { request.Id };
        var idByDate = new Response.GetIdsByDate
        {
            Ids = ids,
            Date = yardPrice.EffectiveDate
        };

        var idsByDate = new List<Response.GetIdsByDate> { idByDate };

        var sendSignalRAndUpdateCache = new BusCommand.SendUpdateCacheBusCommand
        {
            Id = Guid.NewGuid(),
            Description = request.Data.IsToken,
            Name = "Email Notification",
            TimeSpan = DateTime.Now,
            TransactionId = Guid.NewGuid(),
            YardPriceIds = idsByDate,
            Type = request.Data.Type.ToUpper() == "RESERVED"
                ? BookingEnum.RESERVED.ToString()
                : BookingEnum.UNBOOKED.ToString()
        };

        ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri("queue:send-update-cache-queue"));
        await endPoint.Send(sendSignalRAndUpdateCache, cancellationToken);

        return Result.Success();
    }
}
