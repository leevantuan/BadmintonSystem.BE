using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class UpdateBookingCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : ICommandHandler<Command.UpdateBookingCommand, Response.BookingResponse>
{
    public async Task<Result<Response.BookingResponse>> Handle
        (Command.UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Booking booking = await bookingRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                          ?? throw new BookingException.BookingNotFoundException(request.Data.Id);

        booking.BookingDate = request.Data.BookingDate ?? booking.BookingDate;
        booking.BookingTotal = request.Data.BookingTotal ?? booking.BookingTotal;
        booking.UserId = request.Data.UserId ?? booking.UserId;
        booking.SaleId = request.Data.SaleId ?? booking.SaleId;
        booking.BookingStatus =
            request.Data.BookingStatus == 2 ? BookingStatusEnum.Approved : BookingStatusEnum.Cancelled;
        booking.PaymentStatus =
            request.Data.PaymentStatus == 2 ? PaymentStatusEnum.Unpaid : PaymentStatusEnum.Paid;

        Response.BookingResponse? result = mapper.Map<Response.BookingResponse>(booking);

        return Result.Success(result);
    }
}
