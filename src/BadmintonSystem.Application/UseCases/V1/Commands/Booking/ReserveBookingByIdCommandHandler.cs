using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using Response = BadmintonSystem.Contract.Services.V1.Bill.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class ReserveBookingByIdCommandHandler(
    IBookingHub bookingHub,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.ReserveBookingByIdCommand>
{
    public async Task<Result> Handle(Command.ReserveBookingByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardPrice yardPrice = await yardPriceRepository.FindByIdAsync(request.Id, cancellationToken)
                                              ?? throw new YardPriceException.YardPriceNotFoundException(request.Id);

        yardPrice.IsBooking = request.Type.Type.ToUpper() == "RESERVED" ? BookingEnum.RESERVED : BookingEnum.UNBOOKED;

        var ids = new List<Guid> { request.Id };

        await bookingHub.BookingByUserAsync(new Response.BookingHubResponse
        {
            Ids = ids,
            Type = request.Type.Type.ToUpper() == "RESERVED"
                ? BookingEnum.RESERVED.ToString()
                : BookingEnum.UNBOOKED.ToString()
        });

        return Result.Success();
    }
}
