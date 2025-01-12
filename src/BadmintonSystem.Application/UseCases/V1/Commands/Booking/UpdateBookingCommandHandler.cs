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
    : ICommandHandler<Command.UpdateBookingCommand>
{
    public async Task<Result> Handle
        (Command.UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Booking booking = await bookingRepository.FindByIdAsync(request.BookingId, cancellationToken)
                                          ?? throw new BookingException.BookingNotFoundException(request.BookingId);

        booking.BookingStatus = BookingStatusEnum.Approved;
        booking.PaymentStatus = PaymentStatusEnum.Paid;

        return Result.Success();
    }
}
